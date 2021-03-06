﻿const express = require('express');
const socketio = require('socket.io');
const bodyParser = require('body-parser');
const morgan = require('morgan');
const request = require('request');
const events = require('events');
const cors = require('cors');

const app = express();
const io = socketio();
const eventHub = new events.EventEmitter();

app.use(morgan('combined'));
app.use(express.static('static'));
app.use(bodyParser.json());
app.use(cors());
app.options('*', cors());
io.set('origins', '*:*');

app.put('/leave', (req, res) => {
    eventHub.emit('leave', {
        userId: req.body.userId,
        campaignId: req.body.campaignId
    });

    res.status(204).send();
});

app.put('/emit', (req, res) => {
    console.log(`EMIT: action ${req.body.action} triggered for campaign ${req.body.campaignId} with the following payload`);
    console.log(req.body.data);

    // send the message down to the players in the relevant room
    io.sockets.to(req.body.campaignId).emit(req.body.action, req.body.data);

    res.status(204).send();
});

// set up logging
eventHub.on('leave', (data) => {
    console.log(`LEAVE: user ${data.userId} leaving campaign ${data.camapignId}`);
});

// Upon connection, try to authenticate the request and join the user's rooms
io.on('connection', (socket) => {
    let token;
    let campaignId;
    let userId;

    // ensure that a valid token and campaignId were provided in the query string
    try {
        token = socket.request._query.token;
        campaignId = socket.request._query.campaignId;
        const payload = token.split('.')[1];
        const decoded = new Buffer(payload, 'base64').toString();
        userId = JSON.parse(decoded).sub;

        if (!userId) {
            throw new Error('Token does not contain sub property');
        }

        if (!campaignId) {
            throw new Error('Must include campaignId in query string');
        }
    } catch (err) {
        console.error('Request did not contain valid token. Disconnecting...');
        socket.disconnect(true);
        return;
    }

    // register event listeners
    eventHub.on('leave', (data) => {
        if (data.userId.localeCompare(userId) === 0 && data.campaignId.localeCompare(campaignId) === 0) {
            console.log(`Leaving campaign ${campaignId}`);
            socket.leave(campaignId, (err) => {
                if (err) {
                    console.log(`Error leaving ${campaignId}`);
                    return;
                }

                console.log(`Left room ${campaignId}`);
            });
        }
    });

    // Make sure the user is authorized to listen to that campaign
    request.get({
        url: `http://127.0.0.1:5001/micro20/campaigns/${campaignId}`,
        auth: {
            bearer: token
        },
        json: true
    }, (err, response, body) => {
        if (err || response.statusCode !== 200) {
            // dont let unauthenticated hooligans connect to MY socket service!
            console.error(`Error fetching campaign ${campaignId}`);
            console.error(err);
            socket.disconnect(true);
            return;
        }

        console.log(`Joining campaign ${campaignId}`);
        socket.join(campaignId, (err) => {
            if (err) {
                console.log(`Error joining ${campaignId}`);
                return;
            }

            console.log(`Joined room ${campaignId}`);
        });
    });
});

io.listen(app.listen(4000, () => {
    console.log('Server started on port 4000');
}));