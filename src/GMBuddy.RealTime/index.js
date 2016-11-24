const express = require('express');
const socketio = require('socket.io');
const bodyParser = require('body-parser');
const morgan = require('morgan');
const request = require('request');

const app = express();
const io = socketio();

app.use(morgan('combined'));
app.use(express.static('static'));
app.use(bodyParser.json());

app.post('/:campaignId/:action', (req, res) => {
    console.log('\nRequest Received');
    
    const payload = {
        campaignId: req.params.campaignId,
        action: req.params.action,
        data: req.body
    };

    console.log(payload);

    // send the message down to the players in the relevant room
    io.sockets.to(req.params.campaignId).emit(req.params.action, req.body);

    res.status(204).send();
});

// Upon connection, try to authenticate the request and join the user's rooms
io.on('connection', (socket) => {
    let token;

    // ensure that a token was provided in the query string
    try {
        token = socket.request._query.token;
    } catch (err) {
        console.error('Request did not contain token. Disconnecting...');
        socket.disconnect(true);
    }

    // get the campaigns associated with the token
    request.get({
        url: 'http://127.0.0.1:5001/micro20/campaigns',
        auth: {
            bearer: token
        },
        json: true,
    }, (err, response, body) => {
        if (err || response.statusCode != 200) {
            console.error(err);
            socket.disconnect(true);
            return;
        }
        
        for (let campaign of body) {
            console.log('Joining room ' + campaign.campaignId);
            socket.join(campaign.campaignId, (err) => {
                if (err) {
                    console.log('Error joining ' + campaign.campaignId);
                    return;
                }

                console.log('Joined room ' + campaign.campaignId);
            });
        }
    });
});

app.listen(4000, () => {
    console.log('HTTP server running on port 4000');
});

io.listen(4001);