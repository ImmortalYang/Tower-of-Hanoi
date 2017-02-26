using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Towers_of_Hanoi
{
    class DiskMove
    {
        private int diskInd;    //index of the disk making the move 
        private int pegInd;     //the index of new Peg the disk is dropped on

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiskMove()
        {
            diskInd = 0;
            pegInd = 0;
        }

        /// <summary>
        /// Alternative constuctor
        /// @param index number of disk and peg 
        /// </summary>
        /// <param name="dI">index of the disk</param>
        /// <param name="pI">index of the new peg</param>
        public DiskMove(int dI, int pI)
        {
            diskInd = dI;
            pegInd = pI;
        }

        /// <summary>
        /// Alternative constructor
        /// @parm string type of data about disk movement 
        /// </summary>
        /// <param name="move">string of move data. Comma seperated disk index and peg index</param>
        public DiskMove(string move)
        {
            string[] mv = move.Split(',');
            try { 
                diskInd = Convert.ToInt32(mv[0]);
                pegInd = Convert.ToInt32(mv[1]);
            }
            catch (FormatException)
            {
                //this exception will be caught and dealt with in MainForm class
                throw new FormatException("Error in parsing move object");
            }
        }

        /// <summary>
        /// This method converts data of disk index and peg index
        /// in DiskMove object into text style.
        /// e.g. "1, 2" means Disk 1 move to Peg 2.
        /// </summary>
        /// <returns></returns>
        public string AsText()
        {
            string d = (diskInd + 1).ToString();
            string p = (pegInd + 1).ToString();
            string text = "Disk (" + d + ") moved to Peg (" + p + ")";
            return text;
        }

        /// <summary>
        /// Get disk index from DiskMove object.
        /// </summary>
        /// <returns></returns>
        public int GetDiskInd()
        {
            return diskInd;
        }

        /// <summary>
        /// Get peg index number from DiskMove object.
        /// </summary>
        /// <returns></returns>
        public int GetPegInd()
        {
            return pegInd;
        }

        /// <summary>
        /// Convert DiskMove data into the format of "disk index , Peg index"        
        /// </summary>
        /// <returns></returns>
        public string CommaText()
        {
            string d = (diskInd).ToString();
            string p = (pegInd).ToString();
            string text = d.ToString() + "," + p.ToString();
            return text;
        }
    }
}
