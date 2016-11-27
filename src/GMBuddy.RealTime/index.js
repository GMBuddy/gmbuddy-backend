const express = require('express');
const socketio = require('socket.io');
const bodyParser = require('body-parser');
const morgan = require('morgan');
const request = require('request');
const events = require('events');

const app = express();
const io = socketio();
const eventHub = new events.EventEmitter();

app.use(morgan('combined'));
app.use(express.static('static'));
app.use(bodyParser.json());

app.post('/leave/:userId/:campaignId', (req, res) => {
    eventHub.emit('leave', {
        userId: req.params.userId,
        campaignId: req.params.campaignId
    });

    res.status(204).send();
});

app.post('/emit/:campaignId/:action', (req, res) => {
    // send the message down to the players in the relevant room
    io.sockets.to(req.params.campaignId).emit(req.params.action, req.body);

    res.status(204).send();
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
    } catch (err) {
        console.error('Request did not contain valid token. Disconnecting...');
        socket.disconnect(true);
        return;
    }

    // register event listeners
    eventHub.on('leave', (data) => {
        if (data.userId.localeCompare(userId) === 0 && data.camapignId.localeCompare(campaignId) === 0) {
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

    eventHub.on('join', (data) => {
        if (data.userId.localeCompare(userId) === 0 && data.camapignId.localeCompare(campaignId) === 0) {
            console.log(`Joining campaign ${campaignId}`);
            socket.join(campaignId, (err) => {
                if (err) {
                    console.log(`Error joining ${campaignId}`);
                    return;
                }

                console.log(`Joined room ${campaignId}`);
            });
        }
    });

    // Make sure the user is authorized to listen to that campaign
    request.get({
        url: `http://127.0.0.1:5001/micro20/campaigns/${campaignId}`,
        auth: {
            bearer: token
        },
        json: true,
    }, (err, response, body) => {
        if (err || response.statusCode != 200) {
            // dont let unauthenticated hooligans connect to MY socket service!
            console.error(`Error fetching campaign ${campaignId}`);
            console.error(err);
            socket.disconnect(true);
            return;
        }
        
        eventHub.emit('join', {
            userId,
            campaignId
        });
    });
});

app.listen(4000, () => {
    console.log('HTTP server running on port 4000');
});

io.listen(4001);