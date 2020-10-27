exports.PacketBuilder = {
	join(responseID){//,userNameWhite,userNameBlack){
		const packet = Buffer.alloc(5);
		packet.write("JOIN",0);
		packet.writeUInt8(responseID,4);
		//packet.writeUInt8(userNameWhite.length,5);
		//packet.writeUInt8(userNameBlack.length,6);
		//packet.write(userNameWhite,7);
		//packet.write(userNameBlack,7+userNameWhite.length);

		return packet;
	},
	name(responseID){
		const packet = Buffer.alloc(5);
		packet.write("NAME",0);
		packet.writeUInt8(responseID,4);
		return packet

	},init(roleResponse,includeInitGB,game){
		var p1Username;
		var p2Username;

		if(game.clientP1){
			p1Username = game.clientP1.username;

		}else{
			p1Username = "none";

		}


		if(game.clientP2){
			p2Username = game.clientP2.username;

		}else{
			p2Username = "none";

		}



		let firstHalfLength = 8+p1Username.length+p2Username.length;
		let packet;

		const firstHalf = Buffer.alloc(firstHalfLength);

		firstHalf.write("INIT",0);
		//console.log(roleResponse);
		firstHalf.writeUInt8(roleResponse,4);
		//console.log(p1Username.length);
		firstHalf.writeUInt8(p1Username.length,5);
		//console.log(p2Username.length);
		firstHalf.writeUInt8(p2Username.length,6);

		firstHalf.write(p1Username,8);
		firstHalf.write(p2Username,8+p1Username.length);


		if(includeInitGB){
			//console.log("including board");
			firstHalf.writeUInt8(1,7);//write "includes inital board"
			const gbState = this.getBoardStatePacket(game);
			packet = Buffer.concat([firstHalf,gbState],firstHalfLength+64);
		}else{
			//console.log("not including board");
			//write "includes inital board"
			firstHalf.writeUInt8(0,7);
			//console.log(firstHalf.readUInt8(7));
			packet = firstHalf;//Buffer.concat([firstHalf],firstHalfLength);
		}





		return packet;
	},
	check(){
		
		const packet = Buffer.alloc(4);
		packet.write("CHEK",0);
		//packet.writeUInt8(responseID,4);
		return packet


	},
	checkmate(){
		const packet = Buffer.alloc(4);
		packet.write("CKMT",0);
		//acket.writeUInt8(responseID,4);
		return packet

	},
	chat(username,message){
		let usernameLength = username.length;
		let messageLength = message.length; 
		console.log("namelnngth" + usernameLength);
		console.log("mssglngth" + messageLength);
		
		const packet = Buffer.alloc(6 + usernameLength + messageLength);

		packet.write("CHAT",0);
		packet.writeUInt8(usernameLength,4);
		packet.writeUInt8(messageLength,5);

		packet.write(username,6);
		packet.write(message,6 + usernameLength);

		return packet;

	},
	move(errorCode){
		const packet = Buffer.alloc(5);

		packet.write("MOVE",0);
		packet.writeUInt8(errorCode,4);


		return packet;
	},
	update(game){
		// Buffer.alloc(70);

		const firstHalf = Buffer.alloc(6);
		firstHalf.write("UPDT",0);
		firstHalf.writeUInt8(game.whoseTurn,4);
		firstHalf.writeUInt8(game.whoHasWon,5);

		
		const packet = Buffer.concat([firstHalf,this.getBoardStatePacket(game)],70);
		

		return packet;

	},
	hover(x,y){//TODO: Impliment bitmask to validate movesets
		const packet = Buffer.alloc(6);
		packet.write("HOVR",0);
		packet.writeUInt8(x,4);
		packet.writeUInt8(y,5);

		return packet;
	},
	getBoardStatePacket(game){

		const packet = Buffer.alloc(64);

		let offset = 0;

		for(let y = 0; y < game.board.length;y++ ){

				for(let x = 0; x < game.board[y].length;x++ ){
					
					//var stateasnum = this.boardStateToNum(game.board[y][x]);
					packet.writeUInt8(this.boardStateToNum(game.board[y][x]),offset);
					//console.log(`Board state as num at offset:${offset} is ${stateasnum}`);
				
					offset++;

			}			
		}

		return packet;

	},

	boardStateToNum(boardState){

		switch(boardState){
			case 0:
				return 0;
			break;

			case "P1":
				return 1;
			break;

			case "R1":
				return 2;
			break;

			case "N1":
				return 3;
			break;
			
			case "B1":
				return 4;
			break;

			case "Q1":
				return 5;
			break;

			case "K1":
				return 6;
			break;

			case "P2":
				return 7;
			break;

			case "R2":
				return 8;
			break;

			case "N2":
				return 9;
			break;

			case "B2":
				return 10;
			break;

			case "Q2":
				return 11;
			break;

			case "K2":
				return 12;
			break;

			default:
				console.log("Unrecognised board state, returning empty")
				return 0;
			break;



		}
	}

};