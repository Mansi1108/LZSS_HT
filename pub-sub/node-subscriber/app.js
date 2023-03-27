//
// Copyright 2021 The Dapr Authors
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

const express = require('express');
const bodyParser = require('body-parser');
const axios = require('axios');
const lzbase62 = require('lzbase62');
const { MongoClient } = require("mongodb");

//Agregar Mongo
const uri = "mongodb://localhost:27017/";

const client = new MongoClient(uri);

const database = client.db('db-1');
const pubsubTexts = database.collection('pubsubTexts');



const app = express();
// Dapr publishes messages with the application/cloudevents+json content-type
app.use(bodyParser.json({ type: 'application/*+json' }));

const daprPort = process.env.DAPR_HTTP_PORT ?? 3500;
const daprUrl = `http://localhost:${daprPort}/v1.0`;
const pubsubName = 'pubsub';
const port = 3000;

app.get('/dapr/subscribe', (_req, res) => {
    res.json([
        {
            pubsubname: "pubsub",
            topic: "A",
            route: "A"
        },
        {
            pubsubname: "pubsub",
            topic: "B",
            route: "B"
        },
        {
            pubsubname: "pubsub",
            topic: "Compressed",
            route: "Compressed"
        }
    ]);
});

app.post('/A', (req, res) => {
    console.log("A: ", req.body.data.message);
    res.sendStatus(200);
});

app.post('/B', (req, res) => {
    console.log("B: ", req.body.data.message);
    res.sendStatus(200);
});

app.post('/Compressed', async (req, res) => {
    console.log("Decompressed Text: ", req.body.data.message);
    var compressed = lzbase62.compress(req.body.data.message);
    const newBody = {
        messageType: 'resultado',
        message: compressed,
      };
    console.log('Compressed Text: ', compressed);
    const newBody2 = {
        message: compressed,
      };
    await pubsubTexts.insertOne(newBody2);
    await axios.post(`${daprUrl}/publish/${pubsubName}/resultado`, newBody);
    res.sendStatus(200);
});

app.listen(port, () => console.log(`Node App listening on port ${port}!`));
