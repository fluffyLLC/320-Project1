const PacketBuilder = require("./packetBuilder.js").PacketBuilder


exports.Client = class Client{

		constructor(socket, server){
			this.socket = socket;
			this.server = server;
			this.username = "Unknown";

			this.buffer = Buffer.alloc(0);

			this.socket.on("error",e=>this.onError(e));
			this.socket.on("close",()=>this.onClose());
			this.socket.on("data",d=>this.onData(d));

		}

		onError(errmsg){
			console.log("ERROR:" + errmsg);

		}

		onClose(){
			this.server.onClientDisonnect(this);

		}

		onData(data){
			this.buffer = Buffer.concat([this.buffer,data]);
			console.log(this.buffer);
			console.log("data recived");

			if(this.buffer.length < 4) return; // not enough data

			const packetIdentifier = this.buffer.slice(0,4).toString();

			console.log("packet identifyer: " + packetIdentifier);

			switch(packetIdentifier){
				case "JOIN": 
					if(this.buffer.length < 5) return; // not enough data
					const lengthOfUsername = this.buffer.readUInt8(4); //one byte 4 bytes into the buffer (the 5th one)
					if(this.buffer.length < 5 + lengthOfUsername) return;
					const desiredUsername = this.buffer.slice(5,5+lengthOfUsername).toString();

					let responseType = this.server.getUsernameResponse(desiredUsername,this);

					const packet = PacketBuilder.join(responseType);

					this.sendPacket(packet);

					if(responseType <= 3){
						this.username = desiredUsername;
					}
					var usernameWhite = "unknown";
					var usernameBlack = "unknown";

					if(this.server.game.clientP1){
						usernameWhite = this.server.game.clientP1.username;

					}

					if(this.server.game.clientP2){
						usernameBlack = this.server.game.clientP2.username;

					}
					//consume data out of buffer
					//consume(5 + lengthOfUsername);
					this.buffer = this.buffer.slice(5 + lengthOfUsername);

					console.log("desired username: " + desiredUsername);
					//check username

				break;
				case "CHAT": 
					if(this.buffer.length < 6) return;
					const mLength = this.buffer.readUInt8(4);
					console.log(mLength);
					//const mLength = this.buffer.readUInt8(5);

					if(this.buffer.length < 5 + mLength) return;
					var message = this.buffer.toString('utf8',5,5 + mLength) //.slice(4,mLength);

					this.server.broadcastToAll(PacketBuilder.chat(this.username,message));

					this.buffer = this.buffer.slice(5 + mLength);
					//this.username


				break;
				case "PLAY": 
					if(this.buffer.length < 8) return;

					const currentX = this.buffer.readUInt8(4);
					const currentY = this.buffer.readUInt8(5);
					const targetX = this.buffer.readUInt8(6);
					const targetY = this.buffer.readUInt8(7);

					let moveCode = this.server.game.checkMoveValid(currentX,currentY,targetX,targetY);


					//TODO: decouple move & turn checks from data mod and return an error number for invalid moves
					if(moveCode == 0) {

						this.server.game.movePeiceInState(currentX,currentY,targetX,targetY);
						this.server.game.toggleTurn();
						this.server.broadcastToAll(PacketBuilder.update(this.server.game));


					}

					//console.log("user wants to play at: "+x+" "+y);
					

					//this.server.game.playMove(this,x,y);

					this.buffer = this.buffer.slice(8);

					

				break;
				case "HOVR":
					if(this.buffer.length < 6) return;

					const x = this.buffer.readUInt8(4);
					const y = this.buffer.readUInt8(5);
					//TODO:Impliment Bitmask
					if(this.server.game.isClientTurn(this) && this.server.game.checkOwnesPeice(this.server.game.board[y][x])){
						//console.log(this.server.game.board[y][x]);
						this.server.broadcastToAllExcept(PacketBuilder.hover(x,y),this)

					}

					this.buffer = this.buffer.slice(6);

				break;
				default:
				console.log("ERROR: packet identifyer not recognised (" +packetIdentifier+")");
				this.buffer = Buffer.alloc(0);
				break;

			}

		}

		consume(consumeLength){
			
		}

		sendPacket(packet){
			console.log("sending packet: " + packet);
			this.socket.write(packet);
		}


}