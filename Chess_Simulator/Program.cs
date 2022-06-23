// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Chess chess = new();

chess.Main();
public class Chess
{
    public char[,] board = new char[8, 8];
    public void Main()
    {
        Setup();

        Console.WriteLine(CheckScan(new int[] { 0, 0 }, false));

        PrintMatrix(board, flipY: true);
    }


    private void PrintMatrix<T>(T[,] matrix, bool flipX = false, bool flipY = false)
    {
        int xLength = (matrix.GetLength(1) - 1) * Convert.ToInt32(flipX);
        int yLength = (matrix.GetLength(0) - 1) * Convert.ToInt32(flipY);

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            string lineText = "";
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                lineText += Convert.ToString(board[Math.Abs(yLength - i), Math.Abs(xLength - j)]) + " ";
            }
            Console.WriteLine(lineText);

        }
    }

    public void Setup()
    {
        for (int i = 0; i < 8; i++) { for (int j = 0; j < 8; j++) { board[i, j] = ' '; } } // initialize all spaces as empty (a space)

        board[0, 0] = 'R'; // White Rook
        board[0, 7] = 'R'; // White Rook
        board[0, 1] = 'N'; // White knight
        board[0, 6] = 'N'; // White Knight
        board[0, 2] = 'B'; // White Bishop
        board[0, 5] = 'B'; // White Bishop
        board[0, 3] = 'K'; // White King
        board[0, 4] = 'Q'; // White Queen
        for(int i = 0; i < 8; i++) { board[1, i] = 'P'; } // White Pawns


        board[7, 0] = 'r'; // Black Rook
        board[7, 7] = 'r'; // Black Rook
        board[7, 1] = 'n'; // Black knight
        board[7, 6] = 'n'; // Black Knight
        board[7, 2] = 'b'; // Black Bishop
        board[7, 5] = 'b'; // Black Bishop
        board[7, 3] = 'k'; // Black King
        board[7, 4] = 'q'; // Black Queen
        for (int i = 0; i < 8; i++) { board[6, i] = 'p'; } // Black Pawns

    }

    public bool CheckScan(int[] pos, bool isWhite)
    {
        int currentFile;
        int currentRank;

        // pawn checks
        if (isWhite)
        {
            currentRank = pos[1] + 1;
            currentFile = pos[0];
            if (board[currentFile + 1, currentRank] == 'p' || board[currentFile - 1, currentRank] == 'p')
            {
                return true; // in check
            }
        }
        else
        {
            currentRank = pos[1] - 1;
            currentFile = pos[0];
            if (board[currentFile + 1, currentRank] == 'P' || board[currentFile - 1, currentRank] == 'P')
            {
                return true; // in check
            }
        }

        int[][] knightMoves = new int[8][];

            knightMoves[0] = new int[2] { 1, 2 };
            knightMoves[1] = new int[2] { 2, 1 };
            knightMoves[2] = new int[2] { 2, -1 };
            knightMoves[3] = new int[2] { 1, -2 };
            knightMoves[4] = new int[2] { -1, -2 };
            knightMoves[5] = new int[2] { -2, -1 };
            knightMoves[6] = new int[2] { -2, 1 };
            knightMoves[7] = new int[2] { -1, 2 };

        foreach (int[] move in knightMoves)
        {
            currentFile = move[0];
            currentRank = move[1];
            if ((isWhite && board[currentFile, currentRank] == 'n') || !isWhite && board[currentFile, currentRank] == 'N')
            {
                return true; // in check
            }
        }



        bool lineDone;


        int[][] directions = new int[8][];


            directions[0] = new int[2] { 0, 1}; // up
            directions[1] = new int[2] { 1, 1}; // right-up
            directions[2] = new int[2] { 1, 0}; // right
            directions[3] = new int[2] { 1,-1}; // right-down
            directions[4] = new int[2] { 0,-1}; // down
            directions[5] = new int[2] {-1,-1}; // left-down
            directions[6] = new int[2] {-1, 0}; // left
            directions[7] = new int[2] {-1, 1}; // left-up

        foreach (int[] direction in directions)
        {
            string directionType;
            if (direction[0] == 0 || direction[1] == 0) { directionType = "straight"; }
            else { directionType = "diagonal"; }

            lineDone = false;

            currentFile = pos[0] += direction[0];
            currentRank = pos[1] += direction[1];
            for (; currentFile >= 0 && currentFile <= 7 && currentRank >= 0 && currentRank <= 7;)
            {
                switch (CheckScanSquare(currentFile, currentRank, isWhite, directionType))
                {
                    case ("check"):
                        return true; // in check
                    case ("end"):
                        lineDone = true; break;
                }
                if (lineDone) { break; }

                currentFile += direction[0];
                currentRank += direction[1];
            }
        }
        return false; // not in check
    }

    private string CheckScanSquare(int file, int rank, bool isWhite, string direction)
    {
        char currentScan = board[file, rank];
        if (isWhite)
        {
            if (char.IsUpper(currentScan))
            {
                return "end"; // end search of this line
            }

            if (direction == "straight")
            {
                switch (currentScan)
                {
                    case ('r'):
                    case ('q'):
                        return "check"; // in check
                    case ('n'):
                    case ('b'):
                    case ('k'):
                    case ('p'):
                        return "end"; // end search of this line
                }
            }

            else if (direction == "diagonal")
            {
                switch (currentScan)
                {
                    case ('b'):
                    case ('q'):
                        return "check"; // in check
                    case ('n'):
                    case ('r'):
                    case ('k'):
                    case ('p'):
                        return "end";
                }
            }
        }
        else
        {
            if (char.IsLower(currentScan))
            {
                return "end"; // end search of this line
            }
            if (direction == "straight")
            {
                switch (currentScan)
                {
                    case ('R'):
                    case ('Q'):
                        return "check"; // in check
                    case ('N'):
                    case ('B'):
                    case ('K'):
                    case ('P'):
                        return "end"; // end search of this line

                }
            }

            else if (direction == "diagonal")
            {
                switch (currentScan)
                {
                    case ('B'):
                    case ('Q'):
                        return "check"; // in check
                    case ('N'):
                    case ('R'):
                    case ('K'):
                    case ('P'):
                        return "end";
                }
            }
        }
        return "continue";
    }
}