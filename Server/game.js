
exports.Game = {

	whoseTurn:2,
	whoHasWon:0,
	board:[["R1","N1","B1","K1","Q1","B1","N1","R1"],
		   ["P1","P1","P1","P1","P1","P1","P1","P1"],
		   [ 0  , 0  , 0  , 0  , 0  , 0  , 0  , 0  ],
		   [ 0  , 0  , 0  , 0  , 0  , 0  , 0  , 0  ],
		   [ 0  , 0  , 0  , 0  , 0  , 0  , 0  , 0  ],
		   [ 0  , 0  , 0  , 0  , 0  , 0  , 0  , 0  ],
		   ["P2","P2","P2","P2","P2","P2","P2","P2"],
		   ["R2","N2","B2","K2","Q2","B2","N2","R2"],
		   
	],
	clientP1:null, //player 1
	clientP2:null, //player 2
	playMove(client,x,y){
		if(this.whoHasWon > 0) return;//idnore move packets after game has ended
		//if(client != this.clientX && client != this.clientX) return; //ignore spectator move packets

		if(!this.isClientTurn(client)) return;

		if(x < 0 || y < 0) return;//ignore moves off the board

		if(y >= this.board.length) return;//ignore moves off the board
		if(x >= this.board[y].length) return;//ignore moves off the board

		//if(this.board[y][x] > 0) return; //ignore moves on taken spaces

		//this.board[y][x] = this.whoseTurn; //sets board state

		this.whoseTurn = (this.whoseTurn == 1) ? 2 : 1;//toggles turn to next player
		this.checkStateAndUpdate();

	},
	isClientTurn(client){

		if(this.whoseTurn == 1 && client == this.clientP1) return true;
		if(this.whoseTurn == 2 && client == this.clientP2) return true;

		return false;
	},
	movePeiceInState(currentX,currentY,targetX,targetY){
		if(this.board[currentY][currentX] == 0) return false;//return false?
		if(!this.checkOwnesPeice(this.board[currentY][currentX])) return false;
		if(!this.checkStepOnSelf(this.board[targetY][targetX])) return false;

		this.board[targetY][targetX] = this.board[currentY][currentX];
		this.board[currentY][currentX] = 0;
		
		return true;
	},
	checkStepOnSelf(boardState){//return true if we can take the peice in question
		if(boardState == 0) return true;
		return !this.checkOwnesPeice(boardState);

	},checkOwnesPeice(boardState){//return true if we own the peice in question
		
		if(this.whoseTurn == 1){
			if(boardState == "P1" || boardState == "B1" ||boardState == "R1" ||boardState == "K1" ||boardState == "N1" ||boardState == "Q1") return true;

			return false;

		}else if(this.whoseTurn == 2) {
			if(boardState == "P2" || boardState == "B2" ||boardState == "R2" ||boardState == "K2" ||boardState == "N2" ||boardState == "Q2") return true;

			return false;	
		}

		return false;


	},checkStateAndUpdate(){

		//ToDO: look for game over

		const packet = PacketBuilder.update(this);
		Server.broadcastToAll(packet);
		//TODO: send UPDT packet to ALL
	}

}

const Peice = {



}