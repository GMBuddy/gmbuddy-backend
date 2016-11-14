const express = require('express');
const socketio = require('socket.io');
const bodyParser = require('body-parser');

const app = express();

app.use(express.static('static'));
app.use(bodyParser.json());

app.post('/:campaignId/:action', (req, res) => {
    console.log('\nRequest Received');
    
    const payload = {
        campaignId,
        action,
        data: req.body
    };

    console.log(payload);

    res.status(204).send();
});

app.listen(5002, () => {
    console.log('Server running on port 5002');
});