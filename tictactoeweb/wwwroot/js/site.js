class TicTacToe {
    constructor() {
        this.playground = [0, 0, 0, 0, 0, 0, 0, 0, 0];
    }

    playerId = 1;
    moveCounter = 0;
    winIndex = 0;

    switchPlayer() {
        if (this.playerId == 1) {
            this.playerId = 2;
        }
        else {
            this.playerId = 1;
        }
    }

    checkWin() {
        for (let i = 0; i < 9; i += 3) {
            if (this.playground[i] === this.playerId &&
                this.playground[i + 1] === this.playerId &&
                this.playground[i + 2] === this.playerId) {
                return true;
            }
        }

        for (let i = 0; i < 3; i++) {
            if (this.playground[i] === this.playerId &&
                this.playground[i + 3] === this.playerId &&
                this.playground[i + 6] === this.playerId) {
                return true;
            }
        }

        if ((this.playground[0] === this.playerId && this.playground[4] === this.playerId && this.playground[8] === this.playerId) ||
            (this.playground[2] === this.playerId && this.playground[4] === this.playerId && this.playground[6] === this.playerId)) {
            return true;
        }

        return false;
    }

    moveCount() {
        this.moveCounter++;

        if (this.moveCounter == 9) {
            if (this.winIndex == 0) {
                this.winIndex = 3;
            }
        }
    }

    makeMove(cell, userId) {
        if (this.playground[cell] === 0) {
            this.playground[cell] = userId;

            if (this.checkWin()) {
                this.winIndex = userId;

                console.log("Win by player: " + userId);
                return true; // Выход, чтобы не сменять игрока после победы
            }

            this.switchPlayer();
            this.moveCount();

            return true;
        } else {
            console.log("incorrect cell");
            return false;
        }
    }

}