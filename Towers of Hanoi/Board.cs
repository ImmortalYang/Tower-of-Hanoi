using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Towers_of_Hanoi
{
    public class Board
    {
        //a two dimensional array of Disk references that represents the board
        private Disk[,] board; //condition says TWO dimentional array            
        private ArrayList movements;//an ArrayList for storing the moves made
        private Disk[] disks; //Array of disks
        private int baseline; //horizontal base position of pegs and disks (the Location.Y of pnlBase)
        private int[] pegCentreAxis; //centre axis position of each peg

        private const int NUM_DISKS = 4;    //number of disks
        private const int NUM_PEGS = 3;     //number of pegs
        //minimum moves = 2 ^ NUM_DISKS - 1
        private static int MINIMUM_MOVES = Convert.ToInt32(Math.Pow(2, NUM_DISKS) - 1);

        /// <summary>
        /// Initializes a new instance of Board class
        /// </summary>
        public Board()
        {
            board = new Disk[NUM_PEGS, NUM_DISKS];
            movements = new ArrayList();

            //Array of disk objects
            disks = new Disk[NUM_DISKS];
            disks[0] = null;
            disks[1] = null;
            disks[2] = null;
            disks[3] = null;

            //Storing disk object into board array(Two dimensional arrray) 
            board = new Disk[NUM_PEGS, NUM_DISKS]; //condition says TWO dimentional array  

            board[0, 3] = new Disk();
            board[0, 2] = new Disk();
            board[0, 1] = new Disk();
            board[0, 0] = new Disk();

            //Creating arraylist of movement 
            movements = new ArrayList();

            baseline = 0;
            pegCentreAxis = new int[NUM_PEGS];
        }

        /// <summary>
        /// Initializes a new instance of Board class with parameters
        /// </summary>
        /// <param name="d1">disk1</param>
        /// <param name="d2">disk2</param>
        /// <param name="d3">disk3</param>
        /// <param name="d4">disk4</param>
        /// <param name="pnlBase">the reference of the panel correspond to the base. </param>
        /// <param name="lblPegs">labels of pegs</param>
        public Board(Disk d1, Disk d2, Disk d3, Disk d4, Panel pnlBase, Label[] lblPegs)
        {
            //Storing into disks array
            disks = new Disk[NUM_DISKS];
            disks[0] = d1;
            disks[1] = d2;
            disks[2] = d3;
            disks[3] = d4;

            //Storing disk object into board array(Two dimensional arrray) 
            board = new Disk[NUM_PEGS, NUM_DISKS]; //condition says TWO dimentional array  
            board[0, 3] = d1;
            board[0, 2] = d2;
            board[0, 1] = d3;
            board[0, 0] = d4;

            //Arraylist of movement.
            movements = new ArrayList();

            //get the base line of the base panel
            baseline = pnlBase.Location.Y;
            //get the centre line of each peg label
            pegCentreAxis = new int[NUM_PEGS];
    
            //initialize the centre axis position of each peg
            for(int i = 0; i < NUM_PEGS; i++)
            {
                pegCentreAxis[i] = lblPegs[i].Location.X + lblPegs[i].Width / 2;
            }
        }

        /// <summary>
        /// Reset the board to original position
        /// </summary>
        public void Reset()
        {
            for (int iP = 0; iP < NUM_PEGS; iP++)
            {
                //Remove all elements from board array
                for (int iD = 0; iD < NUM_DISKS; iD++)
                {
                    board[iP, iD] = null;

                    //Update disks array
                    disks[iD].setPegNum(0);
                    disks[iD].setLevel(NUM_DISKS - 1 - iD);
                }
            }

            //Reallocate elements 
            board[0, 3] = disks[0]; //Peg 1/Level3 
            board[0, 2] = disks[1]; //Peg 1/Level2 
            board[0, 1] = disks[2]; //Peg 1/Level1
            board[0, 0] = disks[3]; //Peg 1/Level0 

            //Remove all elements from movement arraylist
            movements.Clear();
        }

        /// <summary>
        /// Check if it is valid to begin a move with a particular Disk.
        /// Only the top Disk on a peg can move. 
        /// </summary>
        /// <param name="aDisk">the reference of the disk to be moved</param>
        /// <returns></returns>
        public bool CanStartMove(Disk aDisk)
        {
            //get peg number and the level above this disk
            int pegNum = aDisk.getPegNum();
            int newLevel = aDisk.getLevel() + 1;
            //if this disk is top disk then it can start move
            if(NUM_DISKS == newLevel)
            {
                return true;
            }
            else if (board[pegNum, newLevel] == null)
            {
                return true;
            }
            //if this disk is not top disk then it cannot move
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Check if it is valid to drop a particular disk on a Peg. 
        /// Drops are only allowed for a Disk that is smaller than the top Disk on a peg or for an empty peg.
        /// </summary>
        /// <param name="aDisk">the reference of the disk being dropped</param>
        /// <param name="aPeg">the index of destination peg</param>
        /// <returns></returns>
        public bool CanDrop(Disk aDisk, int aPeg)
        {
            if(aPeg < 0 || aPeg >= NUM_PEGS)
            {
                return false;   //wrong index
            }
            Disk topDisk = null;    //store the top disk on the specific peg
            //find the top disk on this peg
            for(int iD = 0; iD < NUM_DISKS; iD++)
            {
                if (board[aPeg, iD] == null)
                {
                    break;
                }
                topDisk = board[aPeg, iD];
            }
            if (topDisk == null)
            {
                return true;    //can always drop on an empty peg
            }
            else if (topDisk.getDiameter() <= aDisk.getDiameter())
            {
                return false; //cannot drop on a disk with smaller diameter
            }
            else
            {
                return true;  //can only drop on a disk with larger diameter
            }
        }

        /// <summary>
        /// Move a disk to specified peg
        /// </summary>
        /// <param name="aDisk">reference of disk</param>
        /// <param name="pegNum">index of peg</param>
        public void Move(Disk aDisk, int pegNum)  //?second parameter 'newLevel' in original project is ambiguous. Multiple peg can have same newLevel
        {
            //store old position
            int oldPeg = aDisk.getPegNum();
            int oldLvl = aDisk.getLevel();
            //set new postion
            aDisk.setPegNum(pegNum);
            aDisk.setLevel(NewLevInPeg(pegNum));
            board[pegNum, aDisk.getLevel()] = aDisk;
            //clear old position
            board[oldPeg, oldLvl] = null;

            //Save a DiskMove object representing the latest move to the ArrayList of moves
            DiskMove dm = new DiskMove(FindDiskIndex(aDisk), pegNum);
            movements.Add(dm);
        }

        /// <summary>
        /// Return a string giving the moves so far, one move per line.
        /// </summary>
        /// <returns></returns>
        public string AllMovesAsString()
        {
            if(movements.Count == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach(DiskMove dm in movements)
            {
                sb.Append(dm.AsText() + "\r\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Display the current position of the disks. 
        /// </summary>
        public void Display()
        {
            if(disks.Length == 0)
            {
                return;
            }
            foreach(Disk disk in disks)
            {
                //x = centrePosition - diameter / 2
                int locX = pegCentreAxis[disk.getPegNum()] - disk.getDiameter() / 2;
                //y = baseline + diskHeight * (level + 1)
                int locY = baseline - disk.getLabel().Height * (disk.getLevel() + 1);
                disk.getLabel().Location = new Point(locX, locY);
            }
        }

        /// <summary>
        /// Check if a disk center's coordinate is exactly on a given peg
        /// </summary>
        /// <param name="aDisk">reference of the disk</param>
        /// <param name="pegNum">index of the peg</param>
        /// <returns>true if the disk is on the peg, false if disk not on the peg</returns>
        public bool IsOnPeg(Disk aDisk, int pegNum)
        {
            return aDisk.getLabel().Location.X == pegCentreAxis[pegNum] - aDisk.getDiameter() / 2;
        }

        /// <summary>
        /// Check if a disk's coordinate is exactly on a specific level
        /// </summary>
        /// <param name="aDisk">reference of the disk</param>
        /// <param name="level">number of level (0 based)</param>
        /// <returns>true if on that level, false if not</returns>
        public bool IsOnLevel(Disk aDisk, int level)
        {
            return aDisk.getLabel().Location.Y == baseline - aDisk.getLabel().Height * (level + 1);
        }

        /// <summary>
        /// Find the disk object correspond to given label
        /// </summary>
        /// <param name="aLabel">reference of the label</param>
        /// <returns>the disk that correspond to the label. Null if not found</returns>
        public Disk FindDisk(Label aLabel)
        {
            if(disks.Length == 0)
            {
                return null;
            }
            foreach(Disk disk in this.disks)
            {
                if(disk.getLabel() == aLabel)
                {
                    return disk;
                }
            }
            return null;
        }

        /// <summary>
        /// Find the disk object from disk index
        /// </summary>
        /// <param name="diskInd">disk index (0 based)</param>
        /// <returns>disk object</returns>
        public Disk FindDisk(int diskInd)
        {
            try
            {
                return disks[diskInd];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// Find the index of a disk in the disks array
        /// </summary>
        /// <param name="disk">reference of disk</param>
        /// <returns>index in the array. -1 if not found</returns>
        public int FindDiskIndex(Disk disk)
        {
            if(disks.Length == 0)
            {
                return -1;
            }
            for(int i = 0; i < NUM_DISKS; i++)
            {
                if(disks[i] == disk)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Get the next available level of specified peg
        /// </summary>
        /// <param name="pegNum">index of the peg</param>
        /// <returns>the number of level (0 based)</returns>
        public int NewLevInPeg(int pegNum)
        {
            for(int iD = 0; iD < NUM_DISKS; iD++)
            {
                if(board[pegNum, iD] == null)
                {
                    return iD;
                }
            }
            return NUM_DISKS;
        }

        /// <summary>
        /// Get the formatted text of all movements
        /// </summary>
        /// <returns>comma seperated movements, each movement a line</returns>
        public string GetText(/*int cnt*/)
        {
            StringBuilder sb = new StringBuilder();
            if(MoveCount() > 0)
            {
                foreach(DiskMove dm in movements)
                {
                    sb.Append(dm.CommaText() + "\r\n");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get the number of moves so far
        /// </summary>
        /// <returns>move count</returns>
        public int MoveCount()
        {
            return movements.Count;
        }

        /// <summary>
        /// Check if the game is finished and if minimum move is reached
        /// </summary>
        /// <returns>0 if not finished. 1 if finished with minimum move. 2 if finished with more moves than minimum.</returns>
        public int GameFinish()
        {
            foreach(Disk disk in disks)
            {
                if(disk.getPegNum() != NUM_PEGS - 1)
                {
                    return 0;
                }
            }
            if(movements.Count == MINIMUM_MOVES)
            {
                return 1;
            }
            return 2;
        }

        /// <summary>
        /// Load the game file movements data into parameter
        /// </summary>
        /// <param name="dm">diskmove list to be loaded</param>
        public void LoadData(ArrayList dm)
        {
            dm.Clear();
            foreach(DiskMove diskmove in movements)
            {
                dm.Add(new DiskMove(diskmove.CommaText()));
            }
        }
   }
}
