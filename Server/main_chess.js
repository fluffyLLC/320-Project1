const Server = require("./server.js").Server;
const PacketBuilder = require("./packetBuilder.js").PacketBuilder
const Game = require("./game.js").Game;


Server.start(Game);
//console.log(Server);