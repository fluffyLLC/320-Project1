console.log("server.js is running");

var test = "this works"; //will not be accessable
const Client = require("./client.js").Client
const PacketBuilder = require("./packetBuilder.js").PacketBuilder

/*exports = {

	//var test2 = "this actually works"; //no bad
}

/*
exports.Server = class Server {

one posibility
};
*/

exports.Server = { //we can use js object notation too
	port:320,
	clients:[],
	maxConnectedUsers:8,
	start(game){

		this.game = game;
		
		this.socket = require("net").createServer({}, c=>this.onClientConnect(c));
		this.socket.on("error", e=>this.onError(e))
		this.socket.listen({port:this.port},()=>this.onStartLitsen());
		this.socket.on("close", e=>this.onClientDisonnect(e))
	},

	onClientConnect(socket){
		//console.log("new connection from " + socket.localAddress);

		if(this.isServerFull()){

			const packet = PacketBuilder.join(9); // rejected, server full

			socket.end(packet);// end connection with client

		}else{//if server is not full:
			//instantiate new client

			const client = new Client(socket,this);
			this.clients.push(client);
		}

	},

	onError(error){
		console.log("ERROR:" + error);

	}, 

	onStartLitsen(){
		console.log("the server is now listening for connections on port " + this.port);

	},

	onClientDisonnect(client){
		const index = this.clients.indexOf(client); //find object in array

		if(index >= 0) this.clients.splice(index,1); //remove object from array
/*
		if(client == game.clientP1){

			clientP1 = this.clients[1];
			this.client[1].sendPacket(PacketBuilder.join())

		}else if(client == game.clientP2){

			clientP2 = this.clients[1];

		}
*/

	},
	
	isServerFull(){

		return(this.clients.length >= this.maxConnectedUsers);
	},
	//this funcction returns a response ID
	getUsernameResponse(desiredUsername, client){ //TODO: Decouple setting Username and Joining
		//let responseType = 1;


		if(desiredUsername.length <= 4) return 4; //username too short
		if(desiredUsername.length > 15) return 5; //username too long
					//letters (upperandlower)
					//spaces
					//numbers
					//^^^^^^^^^^^these are allowe
		const regexValidChars = /^[a-zA-Z0-9]+$/; //literal regex in JavaScript
		if(!regexValidChars.test(desiredUsername)) return 6;

		const regexProfanity = /(fuck|shit|damn|cunt|faggot|retard|ass|porn|spook)/i;
		if(regexProfanity.test(desiredUsername)) return 8;


		let isUsernameTaken = false;

		this.clients.forEach(c=>{
			if(c == this)return;
			if(c.username == desiredUsername) isUsernameTaken = true;
		});

		if(isUsernameTaken) return 7;

		if(this.game.clientP1 == client) return 1; //you are already p1
		
		if(this.game.clientP2 == client) return 2; //you are already p2
		
		if(this.game.clientP1 && this.game.clientP2) return 3; //spectator
		
		if(!this.game.clientP1){
			this.game.clientP1 = client;
		 	return 1; //you are now p1
		}
		if(!this.game.clientP2){
			this.game.clientP2 = client;
		 	return 2; //you are now p2
		}
		

		return 3; //Spectator

	},
	broadcastToAll(packet){
		this.clients.forEach(c=>{
			//console.log("broadcasting:" + packet);
			//if(c.username){
				c.sendPacket(packet);
			//}
		});

	},broadcastToAllExcept(packet,client){
		this.clients.forEach(c=>{
			//console.log("broadcasting:" + packet);
			if(c != client){
				c.sendPacket(packet);
			}
		});

	},

};