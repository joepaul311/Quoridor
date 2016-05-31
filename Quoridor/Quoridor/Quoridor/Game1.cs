using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Quoridor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D square;
        Texture2D simplesquare;
        Song TurkishMarch;
        Song Leibestraum;
       // Texture2D vbridge;
       // Texture2D hbridge;
       // Texture2D bvbridge;
       // Texture2D bhbridge;
       // Texture2D lava;
       // Texture2D water;
       // Texture2D block2;
        Texture2D AIselectedchip;
        Texture2D selectedchip;
        Texture2D upchip;
        Texture2D downchip;
        Texture2D boardpic;
        Texture2D leftchip;
        Texture2D gameboardpic;
        SpriteFont sf4;
        Texture2D qdite;
        Texture2D rightchip;
        MouseState ms;
        MouseState pms;
        SpriteFont sf;
        SpriteFont sf2;
        SpriteFont sf3;
        wall[] walls;
        KeyboardState kbs;
        KeyboardState prevkbs;
        Vector2 location = new Vector2(5, 8);
        Vector2 plocation = new Vector2(5, 1); //ai location
        const int boardsize = 19;
        Boolean lost = false;
        Boolean won = false;
        Boolean firstisclicked = false;
        //TO ADD:
        //pause after game to see win/loss
        //endgame screen says whether or not you won/lost
        Boolean secondisclicked = false;
        Boolean midturn = false;
        String mode = "main";
        int opponentsquaresmoved = 0;
        int[] choords1 = new int[2];
        int squaresmoved = 0;
        const int CONSTINT = 36;
        int numwalls; //number of available walls to place.
        int[] choords2 = new int[2];
        int prevdifference = 1;
        const int totalwalls = 128;
        int playerwalls = 10;
        int count = 0;
        Random r = new Random();
        int AIwalls = 10;
        int playmode = 0;
        String hsnamestring = "";
        Boolean alreadygone = false;
        String highscorenamelist = "";
        Boolean breached = false;
        String highscorenumlist = "";
        //int numsquares = 0;
        //change the walls added to potential wall list to make better decisions
        public int[,] board = new int[boardsize, boardsize]; // int[x, y]. int[ this is x, as in, how far from left. like coord system, y, how down it is.]
        //board setup:
        /* 0: empty square, can move there
         * 1: wall isn"t on a square, can pass through
         * 2: wall is on a square, cant pass through
         * 10: Player peice
         * 20: Paul Peice
         * 3: selected peice
         */
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.PreferredBackBufferHeight = 364;
            //graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 406;
            graphics.PreferredBackBufferWidth = 625;
            //Console.WriteLine(graphics.PreferredBackBufferHeight);
            //Console.WriteLine(graphics.PreferredBackBufferWidth);
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            walls = new wall[totalwalls];
            kbs = Keyboard.GetState();
            ms = Mouse.GetState();
            
           
            //sets walls
            for (int i = 0; i < boardsize; i++) //fix to incorporate 1,s or empty walls.
                for (int j = 0; j < boardsize; j++)
                    if (i % 2 == 0 || j % 2 == 0) { board[i, j] = 1; }
                    else { board[i, j] = 0; }
            for (int i = 0; i < boardsize; i++) { board[0, i] = 2; board[i, 0] = 2; board[i, boardsize - 1] = 2; board[boardsize - 1, i] = 2; } // sets the main boundaries of walls
            //end sets walls;
            this.IsMouseVisible = true; //One can see the mouse so the walls can be clicked
            //declares and sets the locations
            location.X = 9; 
            location.Y = 17;
            plocation.X = 9;
            plocation.Y = 1;
            board[(int)location.X, (int)location.Y] = 10;
            board[(int)plocation.X, (int)plocation.Y] = 20;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            square = Content.Load<Texture2D>("block"); //chip version of board sprite.
            simplesquare = Content.Load<Texture2D>("tetris block");
            sf = Content.Load<SpriteFont>("SpriteFont1");
            sf2 = Content.Load<SpriteFont>("SpriteFont2");
            sf3 = Content.Load<SpriteFont>("SpriteFont3");
            sf4 = Content.Load<SpriteFont>("SpriteFont4");
            TurkishMarch = Content.Load<Song>("Turkish March_part");
            Leibestraum = Content.Load<Song>("Liebestraum");
            qdite = Content.Load<Texture2D>("reallifequoridorite");
            //hbridge = Content.Load<Texture2D>("Quoridor Horizontal Bridge");
            //vbridge = Content.Load<Texture2D>("Quoridor Vertical Bridge");
            //bhbridge = Content.Load<Texture2D>("Quoridor Broken Horizontal Bridge");
            //bvbridge = Content.Load<Texture2D>("Quoridor Broken Vertical Bridge");
            //lava = Content.Load<Texture2D>("Lava");
            //water = Content.Load<Texture2D>("water");
            //block2 = Content.Load<Texture2D>("block2");
            downchip = Content.Load<Texture2D>("noshadowstaticchip");
            upchip = Content.Load<Texture2D>("noshadowupchip");
            leftchip = Content.Load<Texture2D>("noshadowleftchip");
            rightchip = Content.Load<Texture2D>("noshadowrightchip");
            boardpic = Content.Load<Texture2D>("Quoridorboard");
            gameboardpic = Content.Load<Texture2D>("Quoridorboardpic");
            AIselectedchip = downchip;
            selectedchip = downchip;
            gethsstring();
            //SongCollection s = new SongCollection();
            
           
            MediaPlayer.Play(Leibestraum);
            MediaPlayer.IsRepeating = true;
            
            ////highscorelist = gethsstring();
            // TODO: use this.Content to load your game content here
        }
        protected override void UnloadContent()
        {
        }
        String getMove() //determines player's move
        {
            if ((kbs.IsKeyDown(Keys.Up) && !prevkbs.IsKeyDown(Keys.Up)) || (kbs.IsKeyDown(Keys.W) && !prevkbs.IsKeyDown(Keys.W))) { return "N"; } //if key is up, go north
            if ((kbs.IsKeyDown(Keys.Down) && !prevkbs.IsKeyDown(Keys.Down)) || (kbs.IsKeyDown(Keys.S) && !prevkbs.IsKeyDown(Keys.S))) { return "S"; } //if key is up, go north
            if ((kbs.IsKeyDown(Keys.Left) && !prevkbs.IsKeyDown(Keys.Left)) || (kbs.IsKeyDown(Keys.A) && !prevkbs.IsKeyDown(Keys.A))) { return "W"; } //if key is up, go north
            if ((kbs.IsKeyDown(Keys.Right) && !prevkbs.IsKeyDown(Keys.Right)) || (kbs.IsKeyDown(Keys.D) && !prevkbs.IsKeyDown(Keys.D))) { return "E"; } //if key is up, go north
           
           // if (kbs.IsKeyDown(Keys.Left) && !prevkbs.IsKeyDown(Keys.Left)) { return "W"; } //if key is left, go west
           // if (kbs.IsKeyDown(Keys.Right) && !prevkbs.IsKeyDown(Keys.Right)) { return "E"; } //if key is east, go right
            return "x"; //otherwise, default is x, which is handled later
        }
        Boolean GetSquareClicked() //finds out which squares are clicked for wall placement
        {
            String dir = ""; //determines if the wall is going to be horizontal or vertical
            int x = ms.X; //gets mouse x and y
            int y = ms.Y; //gets mouse x and y
            for (int i = 0; i < boardsize; i++)
            {
                for (int j = 0; j < boardsize; j++)
                {
                    /*
if (i % 2 == 0 && j % 2 == 1) { spriteBatch.Draw(simplesquare, new Rectangle(1+5 * CONSTINT / 8 * i, 1+ -14 + 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT), Color.Red); }
                        else if (i % 2 == 1 && j % 2 == 0) { spriteBatch.Draw(simplesquare, new Rectangle(1+-14 + 5 * CONSTINT / 8 * i, 1+5 * CONSTINT / 8 * j, CONSTINT, CONSTINT / 4), Color.Red); }
                        
                    */
                    int l, r, t, b;
                    l = r = t = b = 0;
                    if (i % 2 == 0 && j % 2 == 1)
                    {
                     //   t = 1 + -14 + 5 * CONSTINT / 8 * j; //left boundary of square
                     //   b = t + CONSTINT; //right boundary of square
                     //   l = 1 + 5 * CONSTINT / 8 * i; //top boundary of square
                     //   r = l + CONSTINT / 4; //bottom boundary of square
                       
                        l = 1 + 5 * CONSTINT / 8 * i; //left boundary of square
                        r = l + CONSTINT/4; //right boundary of square
                        t = 1 + -14 + 5 * CONSTINT / 8 * j; //top boundary of square
                      
                        b = t + CONSTINT; //bottom boundary of square
                       // Console.WriteLine("a");
                    }
                    else if(i%2 == 1 && j%2 == 0)
                    {
                        //issue
                        l = 1 + -14 + 5 * CONSTINT / 8 * i; //left boundary of square
                        r = l + CONSTINT ; //right boundary of square
                        t = 1 + 5 * CONSTINT / 8 * j; //top boundary of square
                        b = t + CONSTINT/4; //bottom boundary of square
                        //Console.WriteLine(l + " " + r);
                        //Console.WriteLine(x + " " + y);
                    }
                    if (l <= x && x <= r && y >= t && y <= b) //if the mouse is inside the square
                    { //i is odd, j is even for wallables (wallables meaning wall can be placed in a square)
                        //Console.WriteLine(l + " " + r);
                        if (firstisclicked == false) //if no walls are selected
                        {
                            if (board[i, j] == 1 && i % 2 == 1 && j % 2 == 0) //if the square is clickable
                            {
                                dir = "h"; //horizontal wall
                                board[i, j] = 3; //board[x,y] is a selected square
                                choords1[0] = i; // first pair of coordinates are set
                                choords1[1] = j; // "
                                firstisclicked = true; //a wall is selected
                            }
                            else if (board[i, j] == 1 && i % 2 == 0 && j % 2 == 1) //same as above, with vertical wall
                            {
                                dir = "v";
                                board[i, j] = 3;
                                choords1[0] = i;
                                choords1[1] = j;
                                firstisclicked = true;
                            }
                        }
                        else if(secondisclicked == false && firstisclicked == true) //if second isnt clicked
                        {
                            if (board[i, j] == 1 && i % 2 == 1 && j % 2 == 0) //same as the if statement above
                            {
                                dir = "h";
                                board[i, j] = 3;
                                choords2[0] = i;
                                choords2[1] = j;
                            }
                            if (board[i, j] == 1 && i % 2 == 0 && j % 2 == 1)
                            {
                                dir = "v";
                                board[i, j] = 3;
                                choords2[0] = i;
                                choords2[1] = j;
                            }
                            secondisclicked = true;
                        }
                    }

                }
            }
            if (secondisclicked && firstisclicked) //if both walls are placed
            {
                if (iswallblocked()) //if the wall being placed doesnt cut off circulation, as in, still allows open path:
                {
                    int avgx = (choords1[0] + choords2[0]) / 2; //gets the middle wall block (a wall is 3 blocks long, choords 1/2 only get the left and right, this gets middle
                    int avgy = (choords1[1] + choords2[1]) / 2;

                    if ((choords1[0] == choords2[0] + 2 && choords1[1] == choords2[1]) || (choords1[0] == choords2[0] - 2 && choords1[1] == choords2[1])) //if this isnt covering any other blocks:
                    {
                        if (dir == "h") //set wall
                        {
                            if (board[avgx, avgy] != 2)
                            {
                                board[avgx, avgy] = 2;
                                board[choords1[0], choords1[1]] = 2;
                                board[choords2[0], choords2[1]] = 2;
                                secondisclicked = false; firstisclicked = false;
                                return true;
                            }
                        }
                    }
                    else if ((choords1[0] == choords2[0] && choords1[1] - 2 == choords2[1]) || (choords1[0] == choords2[0] && choords1[1] + 2 == choords2[1]))
                    { //same as above, with vertical
                        if (dir == "v")
                        {
                            if (board[avgx, avgy] != 2)
                            {
                                board[avgx, avgy] = 2;
                                board[choords1[0], choords1[1]] = 2;
                                board[choords2[0], choords2[1]] = 2;
                                secondisclicked = false; firstisclicked = false;
                                return true;
                            }
                        }
                    }
                    secondisclicked = false; firstisclicked = false; //reset the conditions, and set the temporary walls to permenant walls ( 3 to 1)
                    for (int i = 0; i < boardsize; i++)
                        for (int j = 0; j < boardsize; j++)
                            if (board[i, j] == 3 || board[i,j] == 4)
                                board[i, j] = 1;
                }
                return false;
            }
            return false;
        }
        void setkbsms() //sets keyboard states and mouse states
        {
            pms = ms; //previous mouse state is the current mouse state
            ms = Mouse.GetState(); //current mouse state is new mouse state
            prevkbs = kbs;
            kbs = Keyboard.GetState();
        }
        Boolean mouseclick() //determines if the mouse is clicked.
        {
            return (ms.LeftButton == ButtonState.Pressed && pms.LeftButton == ButtonState.Released);
        }
        Boolean iswallblocked() //determines if the walls cut off a move
        {
            setboardtonorm(); //resets the board to the 1s, 0s, 10s, 20s, and 2s.
            //try
            //{ //if theres an error in finding a path, it means that it is closed off.
            //}
            //catch (Exception e)
            //{
            //Console.WriteLine(e.Message);
            Boolean terminate = false;
            if (!midturn && !lost)
            {
             //   Console.WriteLine(V.getbestmove(plocation, board, 17));
              //  Console.WriteLine(V.getbestmove(location, board, 1));
                setboardtonorm();
                if (V.getbestmove(plocation, board, 17) == "ssapnux")
                {
                    terminate = true;
                    setboardtonorm();
                }
                else
                {
                    setboardtonorm();
                    if (V.getbestmove(location, board, 1) == "ssapnux")
                    {
                        terminate = true;
                        setboardtonorm();

                    }
                    else
                    {
                        setboardtonorm();
                        return true;

                        
                    }
                }

            }
            if(terminate)
            {
                secondisclicked = false; firstisclicked = false; //deletes set walls, and sets back to normal.
                for (int i = 0; i < boardsize; i++)
                    for (int j = 0; j < boardsize; j++)
                        if (board[i, j] == 3 || board[i,j] == 4)
                            board[i, j] = 1;
                setboardtonorm();
                return false;
                //}
            }
            setboardtonorm();
            return true;
        }
        void checkcancelclick() //determines if Q is pressed, meaning to end the wall placement.
        {
            
            if (kbs.IsKeyDown(Keys.Q)) //if keyboard is Q
            {
                secondisclicked = false; firstisclicked = false; //reset the walls.
                for (int i = 0; i < boardsize; i++)
                    for (int j = 0; j < boardsize; j++)
                        if (board[i, j] == 3 || board[i,j] == 4)
                            board[i, j] = 1;
            }
        }
        Boolean playerturn() //all the players turn is done in here
        {
            if (!won)
            {
                
                    if (!midturn && mouseclick() && playerwalls > 0)//if the mouse is clicked
                    {
                       // if (!iswallblocked()) //if the wall placement doesnt cut off a move return true
                       // {
                        //    return false;
                        //}
                        if (GetSquareClicked()) { playerwalls--; return true; } //determine if square is clicked for wall and wall is placed
                    }
                    checkcancelclick(); //see if the wall is quit from
                    if (!firstisclicked) //if a wall's first block isnt placed:
                    {
                        //  Console.WriteLine("premove");
                        String move = getMove(); //get move
                        if (move == "x") { return false; }
                        setboardtonorm(); //resets board
                        if (!doplayermove(ref location, board, move)) { setboardtonorm(); return false; } //if its a legit move, do it.
                        setboardtonorm(); //set board to normal again
                        if (location.Y == 1) { won = true; }
                        squaresmoved++;
                        return true;
                        //}
                    }
                    if (location.Y == 1) { won = true; }
                    return false;
            }
            return false;
        }
        void paulturn() //AI turn
        {
            if (plocation.Y != 17 && !won && !lost) //if he hasn't won (yet)
            {
                //Console.WriteLine("ppremove");
                setboardtonorm(); //reset board
                String pmove = V.getbestmove(plocation, board, 17).Substring(0,2); //get move
                findpotentialwalls();
                if (AIwalls > 0) { if (findbestwall()) { AIwalls--; return; } }
                //Console.WriteLine("ppostmove");
                board[(int)location.X, (int)location.Y] = 10; //place back on the board
                V.domove(ref plocation, board, pmove, 17); //complete move
                opponentsquaresmoved++;
                if (pmove.Substring(0, 1) == "N") { AIselectedchip = upchip; }
                else if (pmove.Substring(0, 1) == "S") { AIselectedchip = downchip; }
                else if (pmove.Substring(0, 1) == "E") { AIselectedchip = rightchip; }
                else if (pmove.Substring(0, 1) == "W") { AIselectedchip = leftchip; }
                setboardtonorm(); //reset board
                if (plocation.Y == 17) { lost = true; } //place winning/losing code here
            }
        }
        void setboardtonorm() //resets board back to original settings, without pathfinding code
        {
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (board[i, j] != 0 && board[i, j] != 1 && board[i, j] != 2 && board[i, j] != 3 && board[i,j] != 4)
                    {
                        board[i, j] = 0;
                    }
                }
            }
            board[(int)plocation.X, (int)plocation.Y] = 20;
            board[(int)location.X, (int)location.Y] = 10;
        }
        Boolean doplayermove(ref Vector2 location2, int[,] boardr, String movee) //Logic for the player move.
        {
            midturn = false;
            String m1 = movee;
            if (m1 == "N")
            {
                if (boardr[(int)location2.X, (int)location2.Y - 1] == 1) //if the space ahead isnt blocked
                {
                    NORTH(ref location2); //go forward 
                    selectedchip = upchip;
                    if (boardr[(int)location2.X, (int)location2.Y] == 20) { midturn = true; return false; } //if the square the player is on is AI, then
                    return true;                                                            // halt the turn and go twice.
                }
            }
            else if (m1 == "S") //same as N but going S direction
            {

                if (boardr[(int)location2.X, (int)location2.Y + 1] == 1)
                {
                    SOUTH(ref location2);
                    selectedchip = downchip;
                    if (boardr[(int)location2.X, (int)location2.Y] == 20) { midturn = true; return false; }
                    return true;
                }
            }
            else if (m1 == "W") //same as N but going in W direction
            {
                if (boardr[(int)location2.X - 1, (int)location2.Y] == 1)
                {
                    WEST(ref location2);
                    selectedchip = leftchip;
                    if (boardr[(int)location2.X, (int)location2.Y] == 20) { midturn = true; return false; }

                    return true;
                    //if 
                }
            }
            else if (m1 == "E") //same as N but going in E direction
            {
                if (boardr[(int)location2.X + 1, (int)location2.Y] == 1)
                {
                    EAST(ref location2);
                    selectedchip = rightchip; 
                    if (boardr[(int)location2.X, (int)location2.Y] == 20) { midturn = true; return false; }
                    return true;
                }
            }
            return false;
        }
        void setusefulwalls()
        {
            setboardtonorm();
            int pdist = V.getbestmove(location, board, 1).Length - 1;
            setboardtonorm();
            int aidist = V.getbestmove(plocation, board, 17).Length - 1;
            setboardtonorm();
            int index = 0;
            for (int i = 1; i < numwalls; i++)
            {
                if (walls[i].playermoves != pdist)
                {
                    walls[index] = walls[i]; index++;
                }           
            }
            numwalls = index; 
        }
        Boolean findbestwall()
        {
            //10 - 5 = 5 ahead
            //5 - 10  = -5 ahead
            setboardtonorm();
            int pdist = V.getbestmove(location, board, 1).Length - 1;
            setboardtonorm();
            int aidist = V.getbestmove(plocation, board, 17).Length - 1;
            setboardtonorm();
            
            
            int difference = walls[0].playermoves - walls[0].AImoves; //higher is better
            //Console.WriteLine("pdist:aidist:dif:prevdif:" + pdist + ":" + aidist + ":" + difference + ":" + prevdifference);
            //Console.WriteLine(walls[0].playermoves + " " + pdist);
            int index = 0;
         
            for (int i = 1; i < numwalls; i++)
            {
                if (walls[i].playermoves - walls[i].AImoves > difference)
                {
                    difference = walls[i].playermoves - walls[i].AImoves; index = i;
                }
            }
            int indexofworst = 0;
            int worstdifference = walls[0].playermoves - walls[0].AImoves;
            for (int i = 1; i < numwalls; i++)
            {
                if (walls[i].playermoves - walls[i].AImoves < worstdifference)
                {
                    worstdifference = walls[i].playermoves - walls[i].AImoves; indexofworst = i;
                }
            }
            //Console.WriteLine(walls[indexofworst].AImoves + " " + walls[indexofworst].playermoves);
            //a few different strategies that are random each game
           // Console.WriteLine(index);
            //Console.WriteLine(prevdifference + " " + difference);
            /*
             * Info we have: 
             * largest difference of ai and player in wall placement
             * Turns to win for AI
             * Turns to win for player
            */
            //include if player moves < ai moves place wall
            Boolean placewall = false;
            //playmode = 0;
            Boolean isadifference = (pdist == (walls[0].playermoves - 1)) && (difference == walls[0].playermoves - walls[0].AImoves);//false;// (walls[0].playermoves - walls[0].AImoves == difference);
            isadifference = !isadifference; //to make it easier to read, it is done in two steps.
            //isadifference means: If theres a difference in distance between the walls placed (not just default value)

            if (playmode == 0)
            {
                placewall = ((difference > prevdifference) || pdist < 2); //first smart ai. BEST SO FAR
                //if the previous turn the the difference was smaller than now, continue enlarging (or place wall)
            }
            else if (playmode == 3)
            {
                placewall = ((difference >= prevdifference) || pdist < 2); //first smart ai. BEST SO FAR
                //if the previous turn the the difference was smaller than now, continue enlarging (or place wall)
            }
            else if (playmode == 1) //further testing required
            { //blitzkrieg //very effective if used on occasion. Weakness: rushing AI with walls will always end in a win
                if (breached)
                {
                    placewall = true;
                }
                else if (pdist < 3 || difference <= -4)
                {
                    breached = true;
                }
            }
            else if (playmode == 5)
            {
                    placewall = ((difference >= prevdifference) || pdist < 2); 
                //semi good
            }
            else if (playmode == 7)//PRACTICE ONLY
            {
                placewall = ((difference < prevdifference) || pdist < 6); //pretty gud
                //if difference is smaller then place wall
            }
            else if (playmode == 6) //UNUSED
            {
                placewall = difference < 0 || pdist < 2; //eh not very good
            }
            else if (playmode == 2) //USED
            {
                placewall = aidist <= 5 || playerwalls <= 5 || pdist < 2; //not too good
            }
            else if (playmode == 4) //doesnt work well
            {
                //int intex;
                int highest = pdist;
                for (int i = 1; i < numwalls; i++)
                {
                   // Console.WriteLine(walls[i].playermoves);
                    if (walls[i].playermoves >= highest)
                    {
                        highest = walls[i].playermoves;
                        index = i;// = walls[i].playermoves - walls[i].AImoves; index = i;
                    }
                }
                if (walls[index].playermoves > pdist)
                {
                    placewall = true;
                }
                else
                {
                    placewall = false;
                }
                //placewall = true;
            }

            
         //   else if (playmode == 3)
         //   {
         //       placewall = walls[index].playermoves < walls[index].AImoves || pdist < 2;// a dud, i think it needs to stop any drastic increasing (effectiveness of1) but unsure why 0 is good.
         //   }
          //  placewall = false;
            Boolean playerhasnowalls = playerwalls == 0;
            if (aidist > 1 && isadifference && (placewall || playerhasnowalls))// !nodifference && ((difference > prevdifference) || pdist < 2))
            {
               // index = indexofworst;
                board[walls[index].x1, walls[index].y1] = 2;
                board[(walls[index].x2 + walls[index].x1) / 2, (walls[index].y2 + walls[index].y1) / 2] = 2;
                board[walls[index].x2, walls[index].y2] = 2;
                
                prevdifference = difference;
                
                return true;
            }
            
            prevdifference = difference;
            return false;
            //board[walls[ptIndex].x1, walls[ptIndex].y1] = 2;
        }
        void findpotentialwalls()
        {
            numwalls = 0;

            for (int j = 0; j < boardsize - 2; j++)
            {
                for (int i = 0; i < boardsize - 2; i++)
                {
                    if (j % 2 == 0 && i % 2 == 1)
                    {
                        if (board[i, j] == 1)
                        { //j2 i == 1
                            if (board[i + 2, j] == 1 && board[i + 1, j] == 1)
                            {
                                board[i, j] = 2; board[i + 1, j] = 2; board[i + 2, j] = 2;
                                if (!iswallblocked())
                                {

                                }
                                else
                                {
                                    walls[numwalls].x1 = i;
                                    walls[numwalls].y1 = j;
                                    walls[numwalls].x2 = i + 2;
                                    walls[numwalls].y2 = j;
                                    // setboardtonorm();
                                    walls[numwalls].playermoves = V.getbestmove(location, board, 1).Length;
                                    setboardtonorm();
                                    walls[numwalls].AImoves = V.getbestmove(plocation, board, 17).Length;
                                    setboardtonorm();
                                    numwalls++;
                                }
                                 board[i, j] = 1; board[i + 1, j] = 1; board[i + 2, j] = 1;
                            }
                        }

                    }
                    if (j % 2 == 1 && i % 2 == 0)
                    {
                        if (board[i, j] == 1)
                        {
                            if (board[i, j + 2] == 1 && board[i, j + 1] == 1)
                            {
                                board[i, j] = 2; board[i, j + 1] = 2; board[i, j + 2] = 2;
                                if (!iswallblocked())
                                {

                                }
                                else
                                {
                                    walls[numwalls].x1 = i;
                                    walls[numwalls].y1 = j;
                                    walls[numwalls].x2 = i;
                                    walls[numwalls].y2 = j + 2;
                                    walls[numwalls].playermoves = V.getbestmove(location, board, 1).Length;
                                    setboardtonorm();
                                    walls[numwalls].AImoves = V.getbestmove(plocation, board, 17).Length;
                                    setboardtonorm();
                                    numwalls++;
                                }

                                 board[i, j] = 1; board[i, j + 1] = 1; board[i, j + 2] = 1;
                            }
                        }
                    }
                }
            }
        }
        void writetohs(String name)
        {
            //hs code
            /*
             * int h1,h2,h3,h4,h5;
             * read in from file
             * if h6 < h1
             * h5 = h4
             * h4 = h3
             * h3 = h2
             * h2 = h1
             * h1 = h6
             * write to file function()
             * break
             * if h6 < h2
             * h5 = h4
             * h4 = 3h
             * h3 = h2
             * h2 = h6
             * write to file functiON()
             * break
             * and repeat for rest
            */
            StreamReader s = new StreamReader("Quoridor.ini");
            String n1, n2, n3, n4, n5, ns1, ns2, ns3, ns4, ns5;
            int s1, s2, s3, s4, s5;
            s.ReadLine();
            n1 = s.ReadLine().Substring(6);
            ns1 = (s.ReadLine());
            n2 = s.ReadLine().Substring(6);
            ns2 = (s.ReadLine());
            n3 = s.ReadLine().Substring(6);
            ns3 = (s.ReadLine());
            n4 = s.ReadLine().Substring(6);
            ns4 = (s.ReadLine());
            n5 = s.ReadLine().Substring(6);
            ns5 = (s.ReadLine());
            s1 = int.Parse(ns1.Substring(8));
            s2 = int.Parse(ns2.Substring(8));
            s3 = int.Parse(ns3.Substring(8));
            s4 = int.Parse(ns4.Substring(8));
            s5 = int.Parse(ns5.Substring(8));

            if (squaresmoved < s1)
            {
                s5 = s4;
                s4 = s3;
                s3 = s2;
                s2 = s1;
                s1 = squaresmoved;
                n5 = n4;
                n4 = n3;
                n3 = n2;
                n2 = n1;
                n1 = hsnamestring;
            }
            else if (squaresmoved < s2)
            {
                s5 = s4;
                s4 = s3;
                s3 = s2;
                s2 = squaresmoved;
                n5 = n4;
                n4 = n3;
                n3 = n2;
                n2 = hsnamestring;
            }
            else if (squaresmoved < s3)
            {
                s5 = s4;
                s4 = s3;
                s3 = squaresmoved;
                n5 = n4;
                n4 = n3;
                n3 = hsnamestring;
            }
            else if (squaresmoved < s4)
            {
                s5 = s4;
                s4 = squaresmoved;
                n5 = n4;
                n4 = hsnamestring;
            }
            else if (squaresmoved < s5)
            {
                s5 = squaresmoved;
                n5 = hsnamestring;
            }
            s.Close();
            StreamWriter write = new StreamWriter("Quoridor.ini", false);
            write.WriteLine("[Quoridor]");
            write.WriteLine("name1=" + n1);
            write.WriteLine("hscore1=" + s1);
            write.WriteLine("name2=" + n2);
            write.WriteLine("hscore2=" + s2);
            write.WriteLine("name3=" + n3);
            write.WriteLine("hscore3=" + s3);
            write.WriteLine("name4=" + n4);
            write.WriteLine("hscore4=" + s4);
            write.WriteLine("name5=" + n5);
            write.WriteLine("hscore5=" + s5);
            write.Close();
        }
        String getkeypressed()
        {
            String s = "";

            if (kbs.IsKeyDown(Keys.A) && !prevkbs.IsKeyDown(Keys.A)) { s += "A"; }
            if (kbs.IsKeyDown(Keys.B) && !prevkbs.IsKeyDown(Keys.B)) { s += "B"; }
            if (kbs.IsKeyDown(Keys.C) && !prevkbs.IsKeyDown(Keys.C)) { s += "C"; }
            if (kbs.IsKeyDown(Keys.D) && !prevkbs.IsKeyDown(Keys.D)) { s += "D"; }
            if (kbs.IsKeyDown(Keys.E) && !prevkbs.IsKeyDown(Keys.E)) { s += "E"; }
            if (kbs.IsKeyDown(Keys.F) && !prevkbs.IsKeyDown(Keys.F)) { s += "F"; }
            if (kbs.IsKeyDown(Keys.G) && !prevkbs.IsKeyDown(Keys.G)) { s += "G"; }
            if (kbs.IsKeyDown(Keys.H) && !prevkbs.IsKeyDown(Keys.H)) { s += "H"; }
            if (kbs.IsKeyDown(Keys.I) && !prevkbs.IsKeyDown(Keys.I)) { s += "I"; }
            if (kbs.IsKeyDown(Keys.J) && !prevkbs.IsKeyDown(Keys.J)) { s += "J"; }
            if (kbs.IsKeyDown(Keys.K) && !prevkbs.IsKeyDown(Keys.K)) { s += "K"; }
            if (kbs.IsKeyDown(Keys.L) && !prevkbs.IsKeyDown(Keys.L)) { s += "L"; }
            if (kbs.IsKeyDown(Keys.M) && !prevkbs.IsKeyDown(Keys.M)) { s += "M"; }
            if (kbs.IsKeyDown(Keys.N) && !prevkbs.IsKeyDown(Keys.N)) { s += "N"; }
            if (kbs.IsKeyDown(Keys.O) && !prevkbs.IsKeyDown(Keys.O)) { s += "O"; }
            if (kbs.IsKeyDown(Keys.P) && !prevkbs.IsKeyDown(Keys.P)) { s += "P"; }
            if (kbs.IsKeyDown(Keys.Q) && !prevkbs.IsKeyDown(Keys.Q)) { s += "Q"; }
            if (kbs.IsKeyDown(Keys.R) && !prevkbs.IsKeyDown(Keys.R)) { s += "R"; }
            if (kbs.IsKeyDown(Keys.S) && !prevkbs.IsKeyDown(Keys.S)) { s += "S"; }
            if (kbs.IsKeyDown(Keys.T) && !prevkbs.IsKeyDown(Keys.T)) { s += "T"; }
            if (kbs.IsKeyDown(Keys.U) && !prevkbs.IsKeyDown(Keys.U)) { s += "U"; }
            if (kbs.IsKeyDown(Keys.V) && !prevkbs.IsKeyDown(Keys.V)) { s += "V"; }
            if (kbs.IsKeyDown(Keys.W) && !prevkbs.IsKeyDown(Keys.W)) { s += "W"; }
            if (kbs.IsKeyDown(Keys.X) && !prevkbs.IsKeyDown(Keys.X)) { s += "X"; }
            if (kbs.IsKeyDown(Keys.Y) && !prevkbs.IsKeyDown(Keys.Y)) { s += "Y"; }
            if (kbs.IsKeyDown(Keys.Z) && !prevkbs.IsKeyDown(Keys.Z)) { s += "Z"; }
            if (kbs.IsKeyDown(Keys.Space) && !prevkbs.IsKeyDown(Keys.Space)) { s += " "; }

            return s;
        }
        Boolean checkhs()
        {
            StreamReader s = new StreamReader("Quoridor.ini");
            String n1, n2, n3, n4, n5;
            int s1, s2, s3, s4, s5;
            s.ReadLine();
            n1 = s.ReadLine();
            n1 = (s.ReadLine());
            n2 = s.ReadLine();
            n2 = (s.ReadLine());
            n3 = s.ReadLine();
            n3 = (s.ReadLine());
            n4 = s.ReadLine();
            n4 = (s.ReadLine());
            n5 = s.ReadLine();
            n5 = (s.ReadLine());
            s1 = int.Parse(n1.Substring(8));
            s2 = int.Parse(n2.Substring(8));
            s3 = int.Parse(n3.Substring(8));
            s4 = int.Parse(n4.Substring(8));
            s5 = int.Parse(n5.Substring(8));
            s.Close();
            if (squaresmoved < s1 || squaresmoved < s2 || squaresmoved < s3 || squaresmoved < s4 || squaresmoved < s5)
            {
                return true;
            }
            return false;
        }
        void gethsstring()
        {
            StreamReader s = new StreamReader("Quoridor.ini");
            String n1, n2, n3, n4, n5, ns1, ns2, ns3, ns4, ns5;
            int s1, s2, s3, s4, s5;
            s.ReadLine();
            n1 = s.ReadLine().Substring(6);
            ns1 = (s.ReadLine());
            n2 = s.ReadLine().Substring(6);
            ns2 = (s.ReadLine());
            n3 = s.ReadLine().Substring(6);
            ns3 = (s.ReadLine());
            n4 = s.ReadLine().Substring(6);
            ns4 = (s.ReadLine());
            n5 = s.ReadLine().Substring(6);
            ns5 = (s.ReadLine());
            s1 = int.Parse(ns1.Substring(8));
            s2 = int.Parse(ns2.Substring(8));
            s3 = int.Parse(ns3.Substring(8));
            s4 = int.Parse(ns4.Substring(8));
            s5 = int.Parse(ns5.Substring(8));
            String hsstring = "";
            hsstring += n1 + "\n";
            hsstring += n2 + "\n";
            hsstring += n3 + "\n";
            hsstring += n4 + "\n";
            hsstring += n5 + "";

            highscorenamelist = hsstring;
            hsstring = "";
            hsstring += s1 + "\n";
            hsstring += s2 + "\n";
            hsstring += s3 + "\n";
            hsstring += s4 + "\n";
            hsstring += s5 + "\n";
            highscorenumlist = hsstring;
            s.Close();
        }
        protected override void Update(GameTime gameTime)
        {
            setkbsms(); //sets the keyboard states
            if (kbs.IsKeyDown(Keys.M) && !prevkbs.IsKeyDown(Keys.M))
            {
                if (MediaPlayer.Volume == 0) { MediaPlayer.Volume = 1; MediaPlayer.Play(Leibestraum); }
                else { MediaPlayer.Volume = 0; MediaPlayer.Stop(); }
            }
            if (mode == "main")
            {
                if (kbs.IsKeyDown(Keys.Enter) && !prevkbs.IsKeyDown(Keys.Enter))
                {
                   breached = false;
                   playmode = r.Next(0, 4);
                   
                   count = 0;
                   Console.WriteLine(playmode);
                    hsnamestring = "";
                    selectedchip = downchip;
                    AIselectedchip = downchip;
                    for (int i = 0; i < boardsize; i++) //fix to incorporate 1,s or empty walls.
                        for (int j = 0; j < boardsize; j++)
                            if (i % 2 == 0 || j % 2 == 0) { board[i, j] = 1; }
                            else { board[i, j] = 0; }
                    for (int i = 0; i < boardsize; i++) { board[0, i] = 2; board[i, 0] = 2; board[i, boardsize - 1] = 2; board[boardsize - 1, i] = 2; } // sets the main boundaries of walls
                    //end sets walls;
                    //this.IsMouseVisible = true; //One can see the mouse so the walls can be clicked
                    //declares and sets the locations
                    location.X = 9;
                    location.Y = 17;
                    plocation.X = 9;
                    plocation.Y = 1;
                    won = false;
                    lost = false;
                    AIwalls = 10;
                    playerwalls = 10;
                    opponentsquaresmoved = 0;
                    prevdifference = 0;
                    squaresmoved = 0;
                    board[(int)location.X, (int)location.Y] = 10;
                    board[(int)plocation.X, (int)plocation.Y] = 20;
                    mode = "game";
                }
                if (kbs.IsKeyDown(Keys.P) && !prevkbs.IsKeyDown(Keys.P))
                {
                    breached = false;
                    //playmode = r.Next(0, 8);
                    count = 0;
                    playmode = 7;
                    //Console.WriteLine(playmode);
                    
                    hsnamestring = "";
                    selectedchip = downchip;
                    AIselectedchip = downchip;
                    for (int i = 0; i < boardsize; i++) //fix to incorporate 1,s or empty walls.
                        for (int j = 0; j < boardsize; j++)
                            if (i % 2 == 0 || j % 2 == 0) { board[i, j] = 1; }
                            else { board[i, j] = 0; }
                    for (int i = 0; i < boardsize; i++) { board[0, i] = 2; board[i, 0] = 2; board[i, boardsize - 1] = 2; board[boardsize - 1, i] = 2; } // sets the main boundaries of walls
                    //end sets walls;
                    //this.IsMouseVisible = true; //One can see the mouse so the walls can be clicked
                    //declares and sets the locations
                    location.X = 9;
                    location.Y = 17;
                    plocation.X = 9;
                    plocation.Y = 1;
                    won = false;
                    lost = false;
                    AIwalls = 10;
                    playerwalls = 10;
                    opponentsquaresmoved = 0;
                    prevdifference = 0;
                    squaresmoved = 0;
                    board[(int)location.X, (int)location.Y] = 10;
                    board[(int)plocation.X, (int)plocation.Y] = 20;
                    mode = "practice";
                }
                if (kbs.IsKeyDown(Keys.I)&&!prevkbs.IsKeyDown(Keys.I)) { mode = "instructionsb"; }
            }
            else if (mode == "practice")
            {
                if (!won && !lost)
                {
                    if (playerturn()) { paulturn(); }
                   
                }
                else if (won)
                {
                    count++;
                    if (count % 75 == 0)
                    {
                        //StreamWriter red = new StreamWriter("Wins vs WHO.txt", true);
                        //red.WriteLine("Robot Mode: " + playmode + " Player Squares moved: " + squaresmoved + "AI Squares Moved: " + opponentsquaresmoved + " Player walls:" + playerwalls + " Opponent Walls: " + AIwalls);
                        //red.Close();
                        //if (checkhs()) { mode = "endgamehs"; }
                        mode = "endgamec"; 
                    }
                }
                else if (lost)
                {
                    count++;
                    if (count % 75 == 0)
                    {
                        //StreamWriter red = new StreamWriter("Player Losses vs WHO.txt", true);
                        //red.WriteLine("Robot Mode: " + playmode + " Player Squares moved: " + squaresmoved + "AI Squares Moved: " + opponentsquaresmoved + " Player walls:" + playerwalls + " Opponent Walls: " + AIwalls);
                        //red.Close();
                        squaresmoved = 0;
                        mode = "endgameb";
                    }
                }
                if (kbs.IsKeyDown(Keys.I) && !prevkbs.IsKeyDown(Keys.I)) { mode = "instructionsc"; }
            }
            else if (mode == "game")
            {

                if (!won && !lost)
                {
                    if (playerturn()) { paulturn(); }
                    //time between code below instead of line above
                    /*   if (!alreadygone)
                       {
                           if (playerturn())
                           {
                               alreadygone = true;
                           } //if the player turn is acceptable, do the AI turn.

                       }
                       else if (alreadygone)
                       {
                           count++;
                           //Console.WriteLine(count + " " + alreadygone);

                        
                           if (count % 25 == 0)
                           {
                               paulturn(); alreadygone = false;
                           }
                       }*/
                }
                else if (won)
                {
                    count++;
                    if (count % 75 == 0)
                    {
                        StreamWriter red = new StreamWriter("Wins vs WHO.txt", true);
                        red.WriteLine("Robot Mode: " + playmode + " Player Squares moved: " + squaresmoved + "AI Squares Moved: " + opponentsquaresmoved + " Player walls:" + playerwalls + " Opponent Walls: " + AIwalls);
                        red.Close();
                        if (checkhs()) { mode = "endgamehs"; }
                        else { mode = "endgamea"; }
                    }
                }
                else if (lost)
                {
                    count++;
                    if (count % 75 == 0)
                    {
                        StreamWriter red = new StreamWriter("Player Losses vs WHO.txt", true);
                        red.WriteLine("Robot Mode: " + playmode + " Player Squares moved: " + squaresmoved + "AI Squares Moved: " + opponentsquaresmoved + " Player walls:" + playerwalls + " Opponent Walls: " + AIwalls);
                        red.Close();
                        squaresmoved = 0;
                        mode = "endgameb";
                    }
                }
                if (kbs.IsKeyDown(Keys.I) && !prevkbs.IsKeyDown(Keys.I)) { mode = "instructionsa"; }
            }
            else if (mode == "endgamea" || mode == "endgameb" || mode == "endgamec")
            {
                //if hs, allow typing

                if (kbs.IsKeyDown(Keys.Enter))
                {
                    gethsstring();
                    mode = "main";
                }
            }
            else if (mode == "endgamehs")
            {
                if (hsnamestring.Length < 11)
                {
                    hsnamestring += getkeypressed();
                }
                if (kbs.IsKeyDown(Keys.Back) && !prevkbs.IsKeyDown(Keys.Back))
                {
                    if (hsnamestring.Length > 0) { hsnamestring = hsnamestring.Substring(0, hsnamestring.Length - 1); }
                }
                if (kbs.IsKeyDown(Keys.Enter) && !prevkbs.IsKeyDown(Keys.Enter))
                {

                    writetohs(hsnamestring);
                    gethsstring();
                    mode = "main";
                }

            }
            else if (mode == "instructionsa" || mode == "instructionsb" || mode == "instructionsc")
            {
                if (kbs.IsKeyDown(Keys.I) && !prevkbs.IsKeyDown(Keys.I))
                {
                    if (mode == "instructionsa")
                    {
                        mode = "game";
                    }
                    else if(mode == "instructionsb")
                    {
                        mode = "main";
                    }
                    else if (mode == "instructionsc")
                    {
                        mode = "practice";
                    }
                }
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (mode != "game" && mode != "practice" &&  mode != "instructionsa" && mode != "instructionsc" && mode != "instructionsb")
            {
                spriteBatch.DrawString(sf2, "Quoridor", new Vector2(200, 0), Color.White);
                if (mode != "main")
                {
                    spriteBatch.DrawString(sf3, "Score: " + squaresmoved, new Vector2(20, 50), Color.White);
                }
                spriteBatch.Draw(qdite, new Rectangle(315, 80, 300, 250), Color.White);
                spriteBatch.DrawString(sf3, highscorenamelist, new Vector2(20, 200), Color.White);
                spriteBatch.DrawString(sf3, highscorenumlist, new Vector2(220, 200), Color.White);
                spriteBatch.DrawString(sf4, "Created by: Joe Bernstein '15\nMusic: Leibestraum - Franz Liszt\nM - Mute Music", new Vector2(350, 350), Color.White);
                if (mode == "endgamea" || mode == "endgameb" || mode == "endgamec") // b is for losing
                {
                    if (mode == "endgameb")
                    {
                        spriteBatch.DrawString(sf, "You lose!", new Vector2(20, 30), Color.White);
                    }
                    if (mode == "endgamec")
                    {
                        spriteBatch.DrawString(sf, "You won! Try the hard mode for a high score.", new Vector2(20, 37), Color.Yellow);
                    }
                    spriteBatch.DrawString(sf, "Enter to Continue", new Vector2(20, 120), Color.White);  
                }
                if (mode == "endgamehs") // b is for winning
                {
                    spriteBatch.DrawString(sf, "Highscore! Type your name,\nand hit enter to continue", new Vector2(20, 120), Color.White);
                    spriteBatch.DrawString(sf, hsnamestring, new Vector2(20, 180), Color.White);
                }
                if (mode == "main") { spriteBatch.DrawString(sf, "Enter to play\nP for Practice Mode\nI for instructions", new Vector2(20, 120), Color.White); }
                
            }
            else if (mode == "instructionsa" || mode == "instructionsb" || mode == "instructionsc")
            {
                spriteBatch.DrawString(sf2, "Quoridor", new Vector2(200, 0), Color.White);
                spriteBatch.DrawString(sf2, "Quoridor", new Vector2(200, 0), Color.White);spriteBatch.DrawString(sf, "Objective: To reach the other end of the\nboard before your opponent.", new Vector2(20, 90), Color.White);
                spriteBatch.DrawString(sf, "\nThis is you:", new Vector2(20, 120), Color.White);
                spriteBatch.Draw(downchip, new Rectangle(145, 140, CONSTINT, CONSTINT), Color.White);
                spriteBatch.DrawString(sf, "\nThis is your opponent:", new Vector2(175, 120), Color.White);
                spriteBatch.Draw(simplesquare, new Rectangle(424, 139, CONSTINT - 9, CONSTINT + 2), Color.White);
                spriteBatch.Draw(downchip, new Rectangle(420, 140, CONSTINT, CONSTINT), Color.Black);
                spriteBatch.DrawString(sf, "\nPlace walls to block your opponent's progress by", new Vector2(20, 150), Color.White);
                spriteBatch.DrawString(sf, "\n\nclicking on two adjacent walls.", new Vector2(20, 150), Color.Yellow);
                spriteBatch.DrawString(sf, "\nMove with the arrow keys. \nTo unselect a wall, press 'Q'", new Vector2(20, 205), Color.White);
                spriteBatch.DrawString(sf, "This is an unblocked (and selectable) wall:", new Vector2(20, 285), Color.White);
                spriteBatch.Draw(simplesquare, new Rectangle(493, 285, CONSTINT / 4, CONSTINT), Color.Gray);
                spriteBatch.DrawString(sf, " or", new Vector2(501, 285), Color.White);
                spriteBatch.Draw(simplesquare, new Rectangle(538, 295, CONSTINT, CONSTINT / 4), Color.Gray);
                spriteBatch.DrawString(sf, "\nJumping on the opponent gives an extra move.", new Vector2(20, 290), Color.White);
                spriteBatch.DrawString(sf, "\n\nPress 'I' to return.", new Vector2(20, 305), Color.White);
            }
            else if(mode == "game" || mode == "practice")
            {
                spriteBatch.DrawString(sf2, "Quoridor", new Vector2(410, 0), Color.White);
                spriteBatch.DrawString(sf, "Press 'I'\nfor instructions", new Vector2(410, 50), Color.Yellow);
                spriteBatch.DrawString(sf, "\n\nPlayer Walls\nRemaining: " + playerwalls.ToString() + "\nOpponent Walls\nRemaining: " + AIwalls.ToString() + "\nPlayer Squares\nMoved: " + squaresmoved.ToString() + "\nOpponent Squares\nMoved: " + opponentsquaresmoved.ToString(), new Vector2(410, 50), Color.White);
                for (int i = 0; i < boardsize; i++)
                {
                    for (int j = 0; j < boardsize; j++)
                    {
                        if (board[i, j] == 0) //empty square to move
                        {
                            if (j == 1)
                            {
                                spriteBatch.Draw(square, new Rectangle(-1 + 5 * CONSTINT / 8 * i - CONSTINT / 3, -1 + 5 * CONSTINT / 8 * j - CONSTINT / 3, CONSTINT, CONSTINT), Color.Green);
                            }
                            else
                            {
                                spriteBatch.Draw(square, new Rectangle(-1 + 5 * CONSTINT / 8 * i - CONSTINT / 3, -1 + 5 * CONSTINT / 8 * j - CONSTINT / 3, CONSTINT, CONSTINT), Color.Brown);
                        
                            }
}
                        else if (board[i, j] == 1) //passable square
                        {
                            Color C = Color.White;
                            if (i % 2 == 0 && j % 2 == 1) { spriteBatch.Draw(simplesquare, new Rectangle(1 + 5 * CONSTINT / 8 * i, 1    + -14 + 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT), C); }
                            else if (i % 2 == 1 && j % 2 == 0) { spriteBatch.Draw(simplesquare, new Rectangle(1 + -14 + 5 * CONSTINT / 8 * i, 1 + 5 * CONSTINT / 8 * j, CONSTINT, CONSTINT / 4), C); }
                            else if (i % 2 == 0 && j % 2 == 0) { spriteBatch.Draw(simplesquare, new Rectangle(5 * CONSTINT / 8 * i, 1 + 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT / 4), C); }
                            else if (i % 2 == 1 && j % 2 == 1) { spriteBatch.Draw(simplesquare, new Rectangle(5 * CONSTINT / 8 * i, 1 + 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT / 4), C); }
                        }
                        else if (board[i, j] == 2) //blocked wall
                        {
                            if (i % 2 == 0 && j % 2 == 1) { spriteBatch.Draw(square, new Rectangle(5 * CONSTINT / 8 * i, 1 + -14 + 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT), Color.Black); }
                            else if (i % 2 == 1 && j % 2 == 0) { spriteBatch.Draw(square, new Rectangle(1 -14 + 5 * CONSTINT / 8 * i, 5 * CONSTINT / 8 * j, CONSTINT, CONSTINT / 4), Color.Black); }
                            else if (i % 2 == 0 && j % 2 == 0) { spriteBatch.Draw(square, new Rectangle(5 * CONSTINT / 8 * i, 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT / 4), Color.Black); }
                            else if (i % 2 == 1 && j % 2 == 1) { spriteBatch.Draw(square, new Rectangle(5 * CONSTINT / 8 * i, 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT / 4), Color.Black); }
                        }
                        else if (board[i, j] == 3) //potential wall
                        {
                            if (i % 2 == 0 && j % 2 == 1) { spriteBatch.Draw(simplesquare, new Rectangle(5 * CONSTINT / 8 * i, 1 + -14 + 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT), Color.Gray); }
                            else if (i % 2 == 1 && j % 2 == 0) { spriteBatch.Draw(simplesquare, new Rectangle(1 + -14 + 5 * CONSTINT / 8 * i, 5 * CONSTINT / 8 * j, CONSTINT, CONSTINT / 4), Color.Gray); }
                            else if (i % 2 == 0 && j % 2 == 0) { spriteBatch.Draw(simplesquare, new Rectangle(5 * CONSTINT / 8 * i, 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT / 4), Color.Gray); }
                            else if (i % 2 == 1 && j % 2 == 1) { spriteBatch.Draw(simplesquare, new Rectangle(5 * CONSTINT / 8 * i, 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT / 4), Color.Gray); }
                        }
                        else if (board[i, j] == 4) //potential wall
                        {
                            if (i % 2 == 0 && j % 2 == 1) { spriteBatch.Draw(simplesquare, new Rectangle(5 * CONSTINT / 8 * i, 1 + -14 + 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT), Color.Blue); }
                            else if (i % 2 == 1 && j % 2 == 0) { spriteBatch.Draw(simplesquare, new Rectangle(1 + -14 + 5 * CONSTINT / 8 * i, 5 * CONSTINT / 8 * j, CONSTINT, CONSTINT / 4), Color.Blue); }
                            else if (i % 2 == 0 && j % 2 == 0) { spriteBatch.Draw(simplesquare, new Rectangle(5 * CONSTINT / 8 * i, 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT / 4), Color.Blue); }
                            else if (i % 2 == 1 && j % 2 == 1) { spriteBatch.Draw(simplesquare, new Rectangle(5 * CONSTINT / 8 * i, 5 * CONSTINT / 8 * j, CONSTINT / 4, CONSTINT / 4), Color.Blue); }
                        }
                        else if (board[i, j] == 10)
                        {
                            if (j != 1)
                            {
                                spriteBatch.Draw(square, new Rectangle(-1 + 5 * CONSTINT / 8 * i - CONSTINT / 3, -1 + 5 * CONSTINT / 8 * j - CONSTINT / 3, CONSTINT, CONSTINT), Color.Brown);
                            }
                            else
                            {
                                spriteBatch.Draw(square, new Rectangle(-1 + 5 * CONSTINT / 8 * i - CONSTINT / 3, -1 + 5 * CONSTINT / 8 * j - CONSTINT / 3, CONSTINT, CONSTINT), Color.Green);
                            
                            }
                            spriteBatch.Draw(selectedchip, new Rectangle(5 * CONSTINT / 8 * i - CONSTINT / 3, 5 * CONSTINT / 8 * j - CONSTINT / 3, CONSTINT, CONSTINT), Color.White);
                          
                        }
                        else if (board[i, j] == 20)
                        {
                            if (j != 1)
                            {
                                spriteBatch.Draw(square, new Rectangle(-1 + 5 * CONSTINT / 8 * i - CONSTINT / 3, -1 + 5 * CONSTINT / 8 * j - CONSTINT / 3, CONSTINT, CONSTINT), Color.Brown);
                            }
                            else
                            {
                                spriteBatch.Draw(square, new Rectangle(-1 + 5 * CONSTINT / 8 * i - CONSTINT / 3, -1 + 5 * CONSTINT / 8 * j - CONSTINT / 3, CONSTINT, CONSTINT), Color.Green);

                            } 
                            spriteBatch.Draw(AIselectedchip, new Rectangle(5 * CONSTINT / 8 * i - CONSTINT / 3, 5 * CONSTINT / 8 * j - CONSTINT / 3, CONSTINT, CONSTINT), Color.Black);
                        }
                    }
                }
                // for (int i = 0; i < boardsize; i++) { board[0, i] = 2; board[i, 0] = 2; board[i, boardsize - 1] = 2; board[boardsize - 1, i] = 2; } // sets the main boundaries of walls
                //  spriteBatch.End();
            }
        
            spriteBatch.End();
            base.Draw(gameTime);
        }
        void NORTH(ref Vector2 v)
        {
            v.Y -= 2;
        }
        void SOUTH(ref Vector2 v)
        {
            v.Y += 2;
        }
        void WEST(ref Vector2 v)
        {
            v.X -= 2;
        }
        void EAST(ref Vector2 v)
        {
            v.X += 2;
        }
    }
}
class V //easy direction changing, also has the shortest path code.
{   
    public static void NORTH(ref Vector2 v) {  v.Y -= 2; }
    public static void SOUTH(ref Vector2 v) { v.Y += 2; }
    public static void WEST(ref Vector2 v) { v.X -= 2; }
    public static void EAST(ref Vector2 v) { v.X += 2; }
    private static String turnsforshortestmoveplayer(Vector2 location, ref int[,] cboard, int endrow)
    {
        // Console.WriteLine("tfsmp");
        for (int i = 0; i < 19; i++)
            for (int j = 0; j < 19; j++)
            {
                if (cboard[i, j] == 0 || cboard[i, j] == 20) { cboard[i, j] = 1000; }
            }
        object end = null;
        Queue<Vector2> que = new Queue<Vector2>();
        que.Enqueue(location);
        //Console.WriteLine("|||||||||||||||||||||||");
        while (que.Count() != 0)
        {
            // Console.WriteLine(que.Peek());    
            Vector2 current = que.Dequeue();

            if (current.Y == endrow) { end = current; break; }
            //North
            // Console.WriteLine((cboard[(int)current.X, (int)current.Y - 2]) + " " + (cboard[(int)current.X, (int)current.Y - 2]));
            if (cboard[(int)current.X, (int)current.Y - 1] == 1 && cboard[(int)current.X, (int)current.Y - 2] > cboard[(int)current.X, (int)current.Y] + 1)
            {
                cboard[(int)current.X, (int)current.Y - 2] = cboard[(int)current.X, (int)current.Y] + 1;
                que.Enqueue(new Vector2(current.X, current.Y - 2));
            }
            //South
            if (cboard[(int)current.X, (int)current.Y + 1] == 1 && cboard[(int)current.X, (int)current.Y + 2] > cboard[(int)current.X, (int)current.Y] + 1)
            {
                cboard[(int)current.X, (int)current.Y + 2] = cboard[(int)current.X, (int)current.Y] + 1;
                que.Enqueue(new Vector2(current.X, current.Y + 2));
            }
            //West
            if (cboard[(int)current.X - 1, (int)current.Y] == 1 && cboard[(int)current.X - 2, (int)current.Y] > cboard[(int)current.X, (int)current.Y] + 1)
            {
                cboard[(int)current.X - 2, (int)current.Y] = cboard[(int)current.X, (int)current.Y] + 1;
                que.Enqueue(new Vector2(current.X - 2, current.Y));
            }
            //East
            if (cboard[(int)current.X + 1, (int)current.Y] == 1 && cboard[(int)current.X + 2, (int)current.Y] > cboard[(int)current.X, (int)current.Y] + 1)
            {
                cboard[(int)current.X + 2, (int)current.Y] = cboard[(int)current.X, (int)current.Y] + 1;
                que.Enqueue(new Vector2(current.X + 2, current.Y));
            }

        }
        if (end == null)
        {
            // Console.WriteLine("end");
            return "unpass";
        }

        //Console.WriteLine(que.Peek());
        int shortest = 1000;
        Vector2 Start = new Vector2(0, 0);

        int startrow = 0;

        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                if (cboard[i, endrow] < shortest && cboard[i, endrow] > 10)
                {
                    shortest = cboard[i, endrow];
                    Start = new Vector2(i, endrow);
                }
            }
        }

        // Console.WriteLine(Start);
        // Console.WriteLine(Start);
        String move = getmoveplayer(cboard, Start, startrow);
        // Console.WriteLine("MOVE: " + move);
        return move;
        //Console.WriteLine(shortest);


    }
    private static String turnsforshortestmove(Vector2 location, ref int[,] cboard, int endrow)
    {

        for (int i = 0; i < 19; i++)
            for (int j = 0; j < 19; j++)
            {
                if (cboard[i, j] == 0 || cboard[i, j] == 10) { cboard[i, j] = 1000; }
            }
        object end = null;
        Queue<Vector2> que = new Queue<Vector2>();
        que.Enqueue(location);
       
        while (que.Count() != 0)
        {
            // Console.WriteLine(que.Peek());    
            Vector2 current = que.Dequeue();

            if (current.Y == endrow) { end = current; break; }
            //North
            if (cboard[(int)current.X, (int)current.Y - 1] == 1 && cboard[(int)current.X, (int)current.Y - 2] > cboard[(int)current.X, (int)current.Y] + 1)
            {
                cboard[(int)current.X, (int)current.Y - 2] = cboard[(int)current.X, (int)current.Y] + 1;
                que.Enqueue(new Vector2(current.X, current.Y - 2));
            }
            //South
            if (cboard[(int)current.X, (int)current.Y + 1] == 1 && cboard[(int)current.X, (int)current.Y + 2] > cboard[(int)current.X, (int)current.Y] + 1)
            {
                cboard[(int)current.X, (int)current.Y + 2] = cboard[(int)current.X, (int)current.Y] + 1;
                que.Enqueue(new Vector2(current.X, current.Y + 2));
            }
            //West
            if (cboard[(int)current.X - 1, (int)current.Y] == 1 && cboard[(int)current.X - 2, (int)current.Y] > cboard[(int)current.X, (int)current.Y] + 1)
            {
                cboard[(int)current.X - 2, (int)current.Y] = cboard[(int)current.X, (int)current.Y] + 1;
                que.Enqueue(new Vector2(current.X - 2, current.Y));
            }
            //East
            if (cboard[(int)current.X + 1, (int)current.Y] == 1 && cboard[(int)current.X + 2, (int)current.Y] > cboard[(int)current.X, (int)current.Y] + 1)
            {
                cboard[(int)current.X + 2, (int)current.Y] = cboard[(int)current.X, (int)current.Y] + 1;
                que.Enqueue(new Vector2(current.X + 2, current.Y));
            }

        }
        if (end == null)
        {
            return "unpass";
        }

        //Console.WriteLine(que.Peek());
        int shortest = 1000;
        Vector2 Start = new Vector2(0, 0);

        for (int i = 0; i < 19; i++)
        {
            if (cboard[i, endrow] < shortest && cboard[i, endrow] > 10)
            {
                shortest = cboard[i, endrow];
                Start = new Vector2(i, endrow);
            }
        }

        int startrow = 0;


        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                if (cboard[i, j] == 20)
                {
                    startrow = j;
                    break;
                }
            }
        }
        // Console.WriteLine(Start + " " + startrow);
        // Console.WriteLine(Start);
        String move = getmove(cboard, Start, startrow);

        return move;
        //Console.WriteLine(shortest);


    }
    static String getmoveplayer(int[,] board, Vector2 Start, int startrow) //startrow is the tracking back to the player
    {
        // Console.WriteLine(Start);
        //Console.WriteLine(board[(int)Start.X, (int)Start.Y + 2]);
        //  Console.WriteLine(Start);
        // Console.WriteLine("gmp");
        if (board[(int)Start.X - 1, (int)Start.Y] == 1)
        {
            if (board[(int)Start.X, (int)Start.Y] - 1 == board[(int)Start.X - 2, (int)Start.Y])
            {
             //   Console.WriteLine("E");
                return "E" + getmoveplayer(board, new Vector2(Start.X - 2, Start.Y), startrow);

            }
        }
        if (board[(int)Start.X + 1, (int)Start.Y] == 1)
        {
            if (board[(int)Start.X, (int)Start.Y] - 1 == board[(int)Start.X + 2, (int)Start.Y])
            {
               // Console.WriteLine("W");
                return "W" + getmoveplayer(board, new Vector2(Start.X + 2, Start.Y), startrow);
            }
        }
        if (board[(int)Start.X, (int)Start.Y + 1] == 1)
        {
            if (board[(int)Start.X, (int)Start.Y] - 1 == board[(int)Start.X, (int)Start.Y + 2])
            {
                //Console.WriteLine("N");
                return "N" + getmoveplayer(board, new Vector2(Start.X, Start.Y + 2), startrow);
            }
        }
        if (board[(int)Start.X, (int)Start.Y - 1] == 1)
        {
            if (board[(int)Start.X, (int)Start.Y] - 1 == board[(int)Start.X, (int)Start.Y - 2])
            {
               // Console.WriteLine("S");
                return "S" + getmoveplayer(board, new Vector2(Start.X, Start.Y - 2), startrow);
            }
        }


        return "";
    }
    static String getmove(int[,] board, Vector2 Start, int startrow) //startrow is the tracking back to the player
    {
        // Console.WriteLine(Start);
        //Console.WriteLine(board[(int)Start.X, (int)Start.Y + 2]);
        //  Console.WriteLine(Start);
        if (board[(int)Start.X - 1, (int)Start.Y] == 1)
        {
            if (board[(int)Start.X, (int)Start.Y] - 1 == board[(int)Start.X - 2, (int)Start.Y])
            {
                return "E" + getmove(board, new Vector2(Start.X - 2, Start.Y), startrow);
            }
        }
        if (board[(int)Start.X + 1, (int)Start.Y] == 1)
        {
            if (board[(int)Start.X, (int)Start.Y] - 1 == board[(int)Start.X + 2, (int)Start.Y])
            {
                return "W" + getmove(board, new Vector2(Start.X + 2, Start.Y), startrow);
            }
        }
        if (board[(int)Start.X, (int)Start.Y + 1] == 1)
        {
            if (board[(int)Start.X, (int)Start.Y] - 1 == board[(int)Start.X, (int)Start.Y + 2])
            {
                return "N" + getmove(board, new Vector2(Start.X, Start.Y + 2), startrow);
            }
        }
        if (board[(int)Start.X, (int)Start.Y - 1] == 1)
        {
            if (board[(int)Start.X, (int)Start.Y] - 1 == board[(int)Start.X, (int)Start.Y - 2])
            {
                return "S" + getmove(board, new Vector2(Start.X, Start.Y - 2), startrow);
            }
        }


        return "";
    }
    private static String inverse(String s)
    {
        if (s == "")
        {
            return s;
        }
        return s.Substring(s.Length - 1) + inverse(s.Substring(0, s.Length - 1)); ;
    }
    public static String getbestmove(Vector2 location, int[,] board, int endrow)
    {
        String move;
        if (endrow == 17) { move = turnsforshortestmove(location, ref board, 17); }
        else { move = turnsforshortestmoveplayer(location, ref board, 1); }
        move = inverse(move);
        move = move + "x";
        return move;//.Substring(0, 2);
    }
    public static Boolean domove(ref Vector2 location, int[,] board, String dir, int endrow) //IGNORECOMMENTwon"t work, I cant edit it"s values like this.
    {
        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                if (board[i, j] != 0 && board[i, j] != 1 && board[i, j] != 2 && board[i, j] != 3 && board[i, j] != 20 && board[i, j] != 4 && board[i, j] != 10)
                {
                    board[i, j] = 0;
                }
            }
        }
        String m1 = dir.Substring(0, 1);
            String m2 = dir.Substring(1, 1);
            if (m1 == "N")
            {
                if (board[(int)location.X, (int)location.Y - 1] == 1)
                {
                    if (board[(int)location.X, (int)location.Y - 2] == 10)
                    {
                        if (m2 == "N") { NORTH(ref location); }
                        if (m2 == "E") { EAST(ref location); }
                        if (m2 == "W") { WEST(ref location); }
                    }
                    NORTH(ref location);
                    return true;
                }  
            }
            else if (m1 == "S")
            {
                if (board[(int)location.X, (int)location.Y + 1] == 1)
                {
                    if (board[(int)location.X, (int)location.Y + 2] == 10)
                    {
                        if (m2 == "S") { SOUTH(ref location); }
                        if (m2 == "E") { EAST(ref location); }
                        if (m2 == "W") { WEST(ref location); }

                    }
                    SOUTH(ref location);
                    return true;
                } 
            }
            else if (m1 == "W")
            {
                if (board[(int)location.X - 1, (int)location.Y] == 1)
                {
                    if (board[(int)location.X - 2, (int)location.Y] == 10)
                    {
                        if (m2 == "S") { SOUTH(ref location); }
                        if (m2 == "W") { WEST(ref location); }
                        if (m2 == "N") { NORTH(ref location); }
                    }
                    WEST(ref location);
                    return true;
                } 
            }
            else if (m1 == "E")
            {
                if (board[(int)location.X + 1, (int)location.Y] == 1)
                {
                    if (board[(int)location.X + 2, (int)location.Y] == 10)
                    {
                        if (m2 == "S") { SOUTH(ref location); }
                        if (m2 == "E") { EAST(ref location); }
                        if (m2 == "N") { NORTH(ref location); }
                    }
                    EAST(ref location);
                    return true;
                }
            }
            return false;
        }
}
struct wall
{
   public int x1;
   public int y1;
   public int x2;
   public int y2;
   public int playermoves;
   public int AImoves;
}