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
		const packet = Buffer.alloc(70);
		packet.write("UPDT",0);
		packet.writeUInt8(game.whoseTurn,4);
		packet.writeUInt8(game.whoHasWon,5);

		let offset = 6;


		for(let y = 0; y < game.board.length;y++ ){

			for(let x = 0; x < game.board[y].length;x++ ){
				
				packet.writeUInt8(this.boardStateToNum(game.board[y][x]),offset);
				
				offset++;

			}			
		}

		return packet;

	},
	hover(x,y){//TODO: Impliment bitmask to validate movesets
		const packet = Buffer.alloc(6);
		packet.write("HOVR",0);
		packet.writeUInt8(x,4);
		packet.writeUInt8(y,5);

		return packet;
	},boardStateToNum(boardState){

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