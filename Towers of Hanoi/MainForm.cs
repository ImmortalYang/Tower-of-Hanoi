using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace Towers_of_Hanoi
{
    public partial class MainForm : Form
    {
        private Board board;    //the board object
        private Disk thisDisk;  //the disk being dragged
        private int pegIndex;   //the peg that is the target of the drop
        private int oldPegIndex;//the peg of thisDisk before dragging
        private int pegTop;     //the top position of pegs
        private ArrayList animMoves; //stores movements in an animation
        private HelpForm help;  //the reference to help info(explain game rules)
       
        /// <summary>
        /// Initializes a new instance of the MainForm class
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            //Initialeze the board
            //Creates 4 Disk objects matching the 4 labels and a Board object
            //and then position the Disks in the starting position
            Disk disk1 = new Disk(lblDisk1, lblDisk1.BackColor.ToArgb(), 3, 0);
            Disk disk2 = new Disk(lblDisk2, lblDisk2.BackColor.ToArgb(), 2, 0);
            Disk disk3 = new Disk(lblDisk3, lblDisk3.BackColor.ToArgb(), 1, 0);
            Disk disk4 = new Disk(lblDisk4, lblDisk4.BackColor.ToArgb(), 0, 0);
            Label[] labels = new Label[] { lblPeg1, lblPeg2, lblPeg3 };
            board = new Board(disk1, disk2, disk3, disk4, pnlBase, labels);
            ResetGame();

            pegTop = lblPeg1.Location.Y;
            animMoves = new ArrayList();   
        }

        /// <summary>
        /// Reset the game to initial state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        /// <summary>
        /// Position the disks in initial position and clear movements
        /// </summary>
        private void ResetGame()
        {
            if (board.MoveCount() > 0)
            {
                //if user has progress then ask them if they want to discard it
                if (DialogResult.Cancel ==
                       MessageBox.Show("Your progress will be discarded. Are you sure?", "Warning", MessageBoxButtons.OKCancel))
                {
                    return;
                }
            }
            board.Reset();
            DisplayGame();
            btnReplay.Enabled = false;
        }

        /// <summary>
        /// Mouse down on a disk: check if it is draggable and wait for drop to be finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disk_MouseDown(object sender, MouseEventArgs e)
        {
            if(animMoves.Count > 0)
            {
                return; //drag is not allowed when playing animation
            }
            Label thisLabel = sender as Label;
            thisDisk = board.FindDisk(thisLabel);
            if (board.CanStartMove(thisDisk))
            {
                DragDropEffects result = thisLabel.DoDragDrop(0, DragDropEffects.All);
                if(result != DragDropEffects.None)
                {
                    //check if user has completed the game after each success drop
                    switch (board.GameFinish())
                    {
                        //finished & minimum move
                        case 1:
                            MessageBox.Show("You have successfully completed the game with the minimum number of moves", "Success");
                            btnReplay.Enabled = true;
                            break;
                        //finished & not minumum move
                        case 2:
                            MessageBox.Show("You have successfully completed the game but not with the minimum number of moves", "Success");
                            btnReplay.Enabled = true;
                            break;
                        //not finished
                        case 0:
                            btnReplay.Enabled = false;
                            break;
                        default:
                            btnReplay.Enabled = false;
                            break;
                    }
                }
            }
            else
            {
                //give an error message if move cannot start
                MessageBox.Show("Sorry, you can not move this disk", "Error");
            }
        }

        /// <summary>
        /// Disk enter a peg, check if it can be dropped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void peg_DragEnter(object sender, DragEventArgs e)
        {
            pegIndex = GetPegIndex(sender as Label);
            if(board.CanDrop(thisDisk, pegIndex))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                //this statement is essential! otherwise it's possible for a user to drop a bigger disk on a smaller one
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Get the index of a peg (0 based) from the label control of that peg
        /// </summary>
        /// <param name="peg">label control of the peg</param>
        /// <returns>index of the peg</returns>
        private int GetPegIndex(Label peg)
        {
            if(lblPeg1 == peg)
            {
                return 0;
            }
            else if(lblPeg2 == peg)
            {
                return 1;
            }
            else if(lblPeg3 == peg)
            {
                return 2;
            }
            return -1;
        }

        /// <summary>
        /// Finish the current drop on the peg
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void peg_DragDrop(object sender, DragEventArgs e)
        {
            board.Move(thisDisk, pegIndex);
            DisplayGame();
        }

        /// <summary>
        /// Exit the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.OK;
            if(board.MoveCount() > 0)
            {
               result = MessageBox.Show("Your progress will be discarded. Are you sure?", "Warning", MessageBoxButtons.OKCancel);
            }
            if (result == DialogResult.OK)
            {
                Close();
            }
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNew_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        /// <summary>
        /// Open a game file and load game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOpen_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ResetGame();
                StreamReader sr = new StreamReader(openFileDialog.FileName); 
                string rdLine = sr.ReadLine();
                while(rdLine != null)
                {
                    //read one line as movement and move the disk
                    try { 
                        DiskMove dm = new DiskMove(rdLine);
                        board.Move(board.FindDisk(dm.GetDiskInd()), dm.GetPegInd());
                    }
                    catch (FormatException)
                    {
                        //file in wrong format or damaged
                        MessageBox.Show("This game file is either in invalid format or is damaged", "Error");
                        board.Reset();
                        DisplayGame();
                        btnReplay.Enabled = false;
                        return;    
                    }     
                    rdLine = sr.ReadLine();
                }

                sr.Close();
                saveFileDialog.FileName = openFileDialog.FileName;

                //display the game result to user
                DisplayGame();
                //if the file is a completed game then enable game replay, otherwise disable replay button
                btnReplay.Enabled = board.GameFinish() > 0;
            }
        }

        /// <summary>
        /// Save the current game file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSave_Click(object sender, EventArgs e)
        {
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                sw.Write(board.GetText());
                sw.Close();
            }
        }

        /// <summary>
        /// Replay a completed game as animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplay_Click(object sender, EventArgs e)
        {
            btnReset.Enabled = false;
            mnuFile.Enabled = false;
            btnReplay.Enabled = false;
            board.LoadData(animMoves);
            board.Reset();
            DisplayGame();
            timerMove.Enabled = true;
        }

        /// <summary>
        /// Each tick means a 'frame' in the animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerAnim_Tick(object sender, EventArgs e)
        {
            //if this disk is on old peg and hasn't reached the top of the peg then move it up
            if(board.IsOnPeg(thisDisk, oldPegIndex) && thisDisk.getLabel().Bottom >= pegTop)
            {
                thisDisk.moveUp();
            }
            //if this disk is on target peg then move down until reaching the target level
            else if(board.IsOnPeg(thisDisk, pegIndex)){
                if(board.IsOnLevel(thisDisk, board.NewLevInPeg(pegIndex)))
                {
                    //the animation of current move is finished when reaching the target level
                    timerAnim.Enabled = false;
                    board.Move(thisDisk, pegIndex);
                    DisplayGame();
                    timerMove.Enabled = true;
                }
                else
                {
                    thisDisk.moveDown();
                }
            }
            //if target peg is on the right then move right
            else if(oldPegIndex < pegIndex)
            {
                thisDisk.moveRight();
            }
            //if target peg is on the left then move left
            else
            {
                thisDisk.moveLeft();
            }
        }

        /// <summary>
        /// Each move per tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerMove_Tick(object sender, EventArgs e)
        {
            timerMove.Enabled = false;
            if(animMoves.Count == 0)
            {
                DisplayGame();
                btnReset.Enabled = true;
                mnuFile.Enabled = true;
                btnReplay.Enabled = true;
                return;
            }
            DiskMove diskmove = (DiskMove)animMoves[0];
            thisDisk = board.FindDisk(diskmove.GetDiskInd());
            pegIndex = diskmove.GetPegInd();
            oldPegIndex = thisDisk.getPegNum();
            animMoves.RemoveAt(0);
            timerAnim.Enabled = true;
        }  

        /// <summary>
        /// Display the current game progress on the interface
        /// </summary>
        private void DisplayGame()
        {
            board.Display();
            lblMoveCnt.Text = board.MoveCount().ToString();
            txtMoves.Text = board.AllMovesAsString();
        }

        /// <summary>
        /// Show a form that explains the game rules and how to play this game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuHelp_Click(object sender, EventArgs e)
        {
            if(this.help == null)
            {
                help = new HelpForm();
            }
            help.ShowDialog();
        }
    }
}
