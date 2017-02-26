using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Towers_of_Hanoi
{
    public partial class HelpForm : Form
    {
        /// <summary>
        /// Initializes an instance of HelpForm
        /// </summary>
        public HelpForm()
        {
            InitializeComponent();
            //text of help information
            lblHelpInfo.Text = "        The Towers of Hanoi game is a very old game where you have a board with three pegs on it. " + 
                "A set of differently sized disks is placed on the first peg with the disks being in order with the smallest on the top. " + 
                "The game is to transfer the disks from the first peg to the third by moving only one disk at a time, only taking disks from the top of any pile and never putting a larger disk on top of a smaller disk. " +
                "The player tries to do this in as few moves as possible.\r\n" + 
                "        Try to complete the game by dragging the disk from one peg to another while adhere to all rules above";
        }

        /// <summary>
        /// Exit help dialogue and return to game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
