const net =  require("net");
const PacketBuilder = require("./packetBuilder.js").PacketBuilder;



//exports.TestClient =
 class TestClinet{

		constructor(){

			this.socket = net.connect({port:320,ip:"127.0.0.1"},this.onStart());

			//this.server = server;
			
			this.role = 2;
			
			this.username = "TestBoi";
			this.message = "This is a test Message!";

			this.buffer = Buffer.alloc(0);

			this.targetX = 8;
			this.targetY = 8;

			this.sendChat = 1;
			this.sendPass = 1;

			this.socket.on("error",e=>this.onError(e));
			this.socket.on("close",()=>this.onClose());
			this.socket.on("data",d=>this.onData(d));
		}

		onData(data){
			this.buffer = Buffer.concat([this.buffer,data]);
			//console.log(this.buffer);
			console.log("TEST data recived");

			let packet = Buffer.alloc(0);

			if(this.buffer.length < 4) return; // not enough data

			const packetIdentifier = this.buffer.slice(0,4).toString();

			console.log("TEST recived packet identifyer: " + packetIdentifier);

			switch(packetIdentifier){
				case "JOIN": 
					let nmLngth = this.username.length;
					packet = Buffer.alloc(5+nmLngth);

					packet.write("NAME",0);
					packet.writeUInt8(nmLngth,4);
					packet.write(this.username,5);

					this.sendPacket(packet);


					this.buffer = Buffer.alloc(0);

				break;
				case "INIT":

					
					this.buffer = Buffer.alloc(0);
				break;
				case "NAME":

					var nmRspns = this.buffer.readUInt8(4);
					console.log("Name Response: " + nmRspns);// this.buffer.slice(4,4).toString());

					packet = Buffer.alloc(5);

					packet.write("INIT",0);
					packet.writeUInt8(this.role,4);
					
					this.sendPacket(packet);

					this.buffer = Buffer.alloc(0);
				break;
				case "CHAT": 

					if(this.sendChat){
 						var messageLngth = this.message.length;
						packet = Buffer.alloc(5+messageLngth);

						packet.write("CHAT",0);
						packet.writeUInt8(messageLngth,4);
						packet.write(this.message,5);
						this.sendPacket(packet);
						this.sendChat = 0;
					}else{
						this.sendChat = 1;
					}

					this.buffer = Buffer.alloc(0);

				break;
				case "UPDT": 

					
					packet = Buffer.alloc(4);
						
					packet.write("PASS",0);
					//packet.writeUInt8(messageLngth,4);
					//packet.write(this.message,5);
					if(this.sendPass){
					setTimeout( ()=>{
						this.sendPacket(packet);
					} ,4000);
					this.sendPass = 0;
				}else{
					this.sendPass = 1;

				}

					

					this.buffer = Buffer.alloc(0);

				break;
				case "HOVR":
					
					this.buffer = Buffer.alloc(0);
					//this.buffer = this.buffer.slice(6);

				break;
				case "PASS":

					this.buffer = Buffer.alloc(0);
				break;
				default:
					console.log("ERROR: packet identifyer not recognised (" +packetIdentifier+")");
					this.buffer = Buffer.alloc(0);
				break;
				



			}
		}

		onError(errMsg){
			console.log(errMsg);
		}

		onClose(){


		}

		onStart(){
			console.log("test client created");

		}

		sendPacket(packet){
			console.log("sending packet: " + packet);
			this.socket.write(packet);
		}
}



let test = new TestClinet;

/*
const socket =  ()=>{
	console.log("connected to server");


	//const username = "Nick";
	const buff  = Buffer.alloc(8);
	buff.write("JOIN");
	buff.writeUInt8(3,4);
	buff.write("Dom",5);
	//socket.write("hello world");
	socket.write(buff);



});

socket.on("error", e=>{
console.log("error: " + e);

});
	*/
