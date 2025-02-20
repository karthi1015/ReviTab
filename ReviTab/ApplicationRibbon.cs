﻿#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Linq;
#endregion

namespace ReviTab
{
    public class Availability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(
          UIApplication a,
          CategorySet b)
        {
            return true;
        }
    }

    [Regeneration(RegenerationOption.Manual)]
    class ApplicationRibbon : IExternalApplication
    {

        public Result OnStartup(UIControlledApplication a)
        {

            try
            {
                               
                AddRibbonPanel(a,"SuperTab");

                RibbonPanel docsPanel = GetSetRibbonPanel(a, "SuperTab", "Documentation");

                RibbonPanel toolsPanel = GetSetRibbonPanel(a, "SuperTab", "Tools");

                RibbonPanel beams = GetSetRibbonPanel(a, "SuperTab", "Structural Framings");

                RibbonPanel walls = GetSetRibbonPanel(a, "SuperTab", "Walls");

                RibbonPanel geometry = GetSetRibbonPanel(a, "SuperTab", "Geometry");

                RibbonPanel commandPanel = GetSetRibbonPanel(a, "SuperTab", "Command Line");

                RibbonPanel zeroState = GetSetRibbonPanel(a, "SuperTab", "Zero State");

                #region Documentation
                
                IList<PushButtonData> stackedButtonsDocumentation = new List<PushButtonData>();

                stackedButtonsDocumentation.Add(CreatePushButton("btnSheetAddCurrentView", "Add View\nto Sheet","","pack://application:,,,/ReviTab;component/Resources/addView.png", "ReviTab.AddActiveViewToSheet",  "Add the active view to a sheet"));

                stackedButtonsDocumentation.Add(CreatePushButton("btnAddMultipleViews", "Add Multiple\nViews","", "pack://application:,,,/ReviTab;component/Resources/addMultipleViews.png", "ReviTab.AddMultipleViewsToSheet", "Add multiple views to a sheet. Select the views in the project browser."));

                stackedButtonsDocumentation.Add(CreatePushButton("btnAddLegends", "Add Legend\nto Sheets", "","pack://application:,,,/ReviTab;component/Resources/legend.png", "ReviTab.AddLegendToSheets",  "Place a legend onto multiple sheets in the same place."));

                stackedButtonsDocumentation.Add(CreatePushButton("btnCreateViewset", "Create\nViewset", "", "pack://application:,,,/ReviTab;component/Resources/createViewSet.png", "ReviTab.CreateViewSet", "Create a Viewset from a list of Sheet Numbers"));

                AddSplitButton(docsPanel, stackedButtonsDocumentation, "DocumentationButton", "Documentation");

                IList<PushButtonData> stackedButtonsSheets = new List<PushButtonData>();

                stackedButtonsSheets.Add(CreatePushButton("btnSetTitleblock", "Set Titleblock\nScale", "pack://application:,,,/ReviTab;component/Resources/rulerSmall.png", "", "ReviTab.SetTitleblockScale", "Set the current sheet titleblock scale to the most used."));

                stackedButtonsSheets.Add(CreatePushButton("btnSetRevCloud", "Rev Cloud\nSummary", "pack://application:,,,/ReviTab;component/Resources/revCloudsmall.png", "", "ReviTab.RevisionCloudsSummary", "Export revision cloud summary"));
                
                stackedButtonsSheets.Add(CreatePushButton("btnUpRevSheet", "Uprev Sheet", "pack://application:,,,/ReviTab;component/Resources/UprevSmall.png", "", "ReviTab.UpRevSheet", "Up rev the current sheet. It copies the content from the previous revision excluding the date"));

                AddStackedButton(docsPanel, stackedButtonsSheets, "SheetsButton", "Sheets");


                #endregion


                #region Tools



                if (AddPushButton(toolsPanel, "btnCreateSections", "Create Multiple" + Environment.NewLine + "Sections", "", "pack://application:,,,/ReviTab;component/Resources/multipleSections.png", "ReviTab.CreateSections", "Create multiple sections from selected elements (must have location curve i.e. beams, walls, lines...)") == false)
                {
                    MessageBox.Show("Failed to add button Create Multiple Sections", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnSelectText", "Select All" + Environment.NewLine + "Text", "", "pack://application:,,,/ReviTab;component/Resources/selectText.png", "ReviTab.SelectAllText", "Select all text notes in the project. Useful if you want to run the check the spelling.") == false)
                {
                    MessageBox.Show("Failed to add button Select all text", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //Override settings

                if (AddPushButton(toolsPanel, "btnSwapGrid", "Swap Grid" + Environment.NewLine + "Head", "", "pack://application:,,,/ReviTab;component/Resources/swapGrids.png", "ReviTab.SwapGridBubbles", "Swap the head of the selected grids") == false)
                {
                    MessageBox.Show("Failed to add button Swap Grid Head", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnOverrideDimensions", "Override \nDimension", "", "pack://application:,,,/ReviTab;component/Resources/dimensionOverride.png", "ReviTab.OverrideDimensions", "Override the text of a dimension") == false)
                {
                    MessageBox.Show("Failed to add button Swap Grid Head", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(toolsPanel, "btnCopyLinkedElements", "Copy Linked \nElements", "", "pack://application:,,,/ReviTab;component/Resources/copyLinked.png", "ReviTab.CopyLinkedElements", "Copy elements from linked models") == false)
                {
                    MessageBox.Show("Failed to add button Copy Linked Elements", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                IList<PushButtonData> filterSelection = new List<PushButtonData>();
                
                filterSelection.Add(CreatePushButton("selBeams", "Select Beams", "", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "ReviTab.FilterSelectionBeams", "Select Beams Only"));

                filterSelection.Add(CreatePushButton("selColumns", "Select Columns", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "ReviTab.FilterSelectionColumns", "Select Columns Only"));

                filterSelection.Add(CreatePushButton("selDim", "Select Dimensions", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "ReviTab.FilterSelectionDimensions", "Select Dimensions Only"));

                filterSelection.Add(CreatePushButton("selGrids", "Select Grids", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "ReviTab.FilterSelectionGrids", "Select Grids Only"));

                filterSelection.Add(CreatePushButton("selLines", "Select Lines", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "ReviTab.FilterSelectionLines", "Select Lines Only"));

                filterSelection.Add(CreatePushButton("selTags", "Select Tags", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "ReviTab.FilterSelectionTags", "Select Tags Only"));

                filterSelection.Add(CreatePushButton("selText", "Select Text", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "ReviTab.FilterSelectionText", "Select Text Only"));

                filterSelection.Add(CreatePushButton("selWalls", "Select Walls", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "ReviTab.FilterSelectionWalls", "Select Walls Only"));

                AddSplitButton(toolsPanel, filterSelection, "filterSelection", "Filter Selection");

                if (AddPushButton(toolsPanel, "btnPrintSelected", "Print Selected", "", "pack://application:,,,/ReviTab;component/Resources/backgroundPrint.png", "ReviTab.PrintSelected", "Print the selected Sheets in the Project Browsser") == false)
                {
                    MessageBox.Show("Failed to add button Print Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                #endregion

                #region Structural Framing

                IList<PushButtonData> stackedButtonsCQT = new List<PushButtonData>();

                stackedButtonsCQT.Add(CreatePushButton("btnPlaceVoidByFace", "Place Void" + Environment.NewLine + "By Face", "pack://application:,,,/ReviTab;component/Resources/addBeamOpeningSmall.png", "", "ReviTab.VoidByFace", "Place a void on a beam face"));

                stackedButtonsCQT.Add(CreatePushButton("btnPlaceVoidByLine", "Void By Line", "pack://application:,,,/ReviTab;component/Resources/lineSmall.png", "", "ReviTab.VoidByLine", "Place a void at line beam intersection. Contact: Ethan Gear."));

                stackedButtonsCQT.Add(CreatePushButton("btnPlaceTags", "Place Tags", "pack://application:,,,/ReviTab;component/Resources/tagSmall.png", "", "ReviTab.AddTagsApplyUndo", "Place a tag on multiple beams"));

                AddStackedButton(beams, stackedButtonsCQT, "CQTButton", "CQT");




                if (AddPushButton(beams, "btnMoveBeamEnd", "Move Beam End", "", "pack://application:,,,/ReviTab;component/Resources/movement-arrows.png", "ReviTab.MoveBeamEnd", "Move a beam endpoint to match a selected beam closest point") == false)
                {
                    MessageBox.Show("Failed to add button Move Beam End", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(beams, "btnChangeBeamLocation", "Change Beam" + Environment.NewLine + "Location", "", "pack://application:,,,/ReviTab;component/Resources/changebeamlocation.png", "ReviTab.ChangeBeamLocation", "Move a beam to new location.") == false)
                {
                    MessageBox.Show("Failed to add button change beam location", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(beams, "btnEditJoin", "Edit Beam" + Environment.NewLine + "End Join", "", "pack://application:,,,/ReviTab;component/Resources/joinEnd.png", "ReviTab.EditBeamJoin", "Allow/Disallow beam end join") == false)
                {
                    MessageBox.Show("Failed to add button Edit Beam", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                #endregion

                #region Splitter

                if (AddPushButton(walls, "btnWallSplitter", "Split Wall" + Environment.NewLine + "By Levels", "", "pack://application:,,,/ReviTab;component/Resources/splitWalls.png", "ReviTab.WallSplitter", "Split a wall by levels. NOTE: The original wall will be deleted.") == false)
                {
                    MessageBox.Show("Failed to add button Split Wall", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(walls, "btnColumnSplitter", "Split Column" + Environment.NewLine + "By Levels", "", "pack://application:,,,/ReviTab;component/Resources/columnSplit.png", "ReviTab.ColumnSplitter", "Split a wall by levels. NOTE: The original wall will be deleted.") == false)
                {
                    MessageBox.Show("Failed to add button Split Column", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                #endregion

                #region Geometry

                IList<PushButtonData> stackedButtonsGroupGeometry = new List<PushButtonData>();

                stackedButtonsGroupGeometry.Add(CreatePushButton("btnSATtoDS", "Element to DirectShape", "pack://application:,,,/ReviTab;component/Resources/flatten.png", "", "ReviTab.SATtoDirectShape", "Convert an element into a DirectShape. Deletes the original element."));

                stackedButtonsGroupGeometry.Add(CreatePushButton("btnProjectLines", "Project Lines to Surface", "pack://application:,,,/ReviTab;component/Resources/projectLine.png", "", "ReviTab.ProjectLines", "Project some lines onto a surface."));

                stackedButtonsGroupGeometry.Add(CreatePushButton("btnDrawAxis", "Draw Axis", "pack://application:,,,/ReviTab;component/Resources/axis.png", "", "ReviTab.DrawObjectAxis", "Draw local and global axis on a point on a surface."));

                AddStackedButton(geometry, stackedButtonsGroupGeometry, "GeometryButton", "Geometry");

                #endregion
                
                #region Zero State

                if (AddZeroStatePushButton(zeroState, "btnClaritySetup", "Clarity Setup", "", "pack://application:,,,/ReviTab;component/Resources/claSetup.png", "ReviTab.ClaritySetup", "Open a model in background and create a 3d view for Clarity IFC export.", "ReviTab.Availability") == false)
                {
                    MessageBox.Show("Failed to add button Clarity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                IList<PushButtonData> stackedButtonsDBPurgeBackPrint = new List<PushButtonData>();

                stackedButtonsDBPurgeBackPrint.Add(CreatePushButton("btnPush", "Push to DB", "pack://application:,,,/ReviTab;component/Resources/arrowUpSmall.png", "","ReviTab.PushToDB", "Push date, user, rvtFileSize, elementsCount, typesCount, sheetsCount, viewsCount, viewportsCount, warningsCount to 127.0.0.1"));

                stackedButtonsDBPurgeBackPrint.Add(CreatePushButton("btnPurgeFamilies", "Purge Families", "pack://application:,,,/ReviTab;component/Resources/wipingSmall.png","", "ReviTab.PurgeFamily", "Purge families and leave only a type called Default", "ReviTab.Availability"));

                stackedButtonsDBPurgeBackPrint.Add(CreatePushButton("btnPrintBackground", "Background" + Environment.NewLine + "Print", "pack://application:,,,/ReviTab;component/Resources/backgroundPrint.png", "","ReviTab.PrintInBackground", "Open a model in background and print the selcted drawings", "ReviTab.Availability"));

                AddStackedButton(zeroState, stackedButtonsDBPurgeBackPrint, "PurgeCommands", "Purge");


                /*
                if (AddZeroStatePushButton(zeroState, "btnDiasbleWarning", "Open No Warnings", "", "pack://application:,,,/ReviTab;component/Resources/addMultiViews.png", "ReviTab.SuppressWarnings", "Suppress warnings when opening files", "ReviTab.Availability") == false)
                {
                    MessageBox.Show("Failed to add button Purge Families", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                */

                IList<PushButtonData> stackedButtonsGroupMetadata = new List<PushButtonData>();

                stackedButtonsGroupMetadata.Add(CreatePushButton("btnHowl", "Howl", "pack://application:,,,/ReviTab;component/Resources/ghowlicon16x16.png", "", "ReviTab.Howl", "Howl"));

                stackedButtonsGroupMetadata.Add(CreatePushButton("btnAddMetadata", "Add Metadata", "pack://application:,,,/ReviTab;component/Resources/AddMetadataIcon.png", "", "ReviTab.AddPDFcustomProperties", "Add custom properties to a list of pdfs"));
                
                stackedButtonsGroupMetadata.Add(CreatePushButton("btnInfo", "Info", "pack://application:,,,/ReviTab;component/Resources/info16x16.png", "", "ReviTab.VersionInfo", "Display Version Info Task Dialog.", "ReviTab.Availability"));

                AddStackedButton(zeroState, stackedButtonsGroupMetadata, "MetadataCommands", "Metadata");


                #endregion

                if (AddTextBox(commandPanel, "btnCommandLine", "*Structural Framing+Length>10000 \n *Walls+Mark!aa \n sheets: all \n sheets: A101 A103 A201\n tblocks: all") == false)
                {
                    MessageBox.Show("Failed to add Text Box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                // Return Success
                return Result.Succeeded;
            }
            catch
            {
                // In Case of Failure
                return Result.Failed;
            }


        }

        public Result OnShutdown(UIControlledApplication a)
        {
            try
            {
                // Begin Code Here
                // Return Success
                return Result.Succeeded;
            }
            catch
            {
                // In Case of Failure
                return Result.Failed;
            }
        }

        /// <summary>
        /// Create a Ribbon Tab and a Ribbon Panel
        /// </summary>
        /// <param name="application"></param>
        /// <param name="tabName"></param>
        /// <param name="ribbonName"></param>
        static void AddRibbonPanel(UIControlledApplication application, string tabName)

        {

            // Create a custom ribbon tab

            application.CreateRibbonTab(tabName);



            // Add a new ribbon panel

            //RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, ribbonName);

            /*

            // Get dll assembly path

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;



            // create push button for CurveTotalLength

            PushButtonData b1Data = new PushButtonData(

                "cmdCurveTotalLength",

                "Total" + System.Environment.NewLine + "  Length  ",

                thisAssemblyPath,

                "ReviTab.CommandHelloWorld");

            //typeof(CommandHelloWorld).FullName

            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;

            pb1.ToolTip = "Select Multiple Lines to Obtain Total Length";

            //BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/GrimshawRibbon;component/Resources/totalLength.png"));

            //pb1.LargeImage = pb1Image;*/

        }

        /// <summary>
        /// Selects or Creates a Ribbon Panel in a specified Ribbon Tab
        /// </summary>
        /// <param name="application"></param>
        /// <param name="tabName"></param>
        /// <param name="panelName"></param>
        /// <returns></returns>
        private RibbonPanel GetSetRibbonPanel(UIControlledApplication application, string tabName, string panelName)
        {
            List<RibbonPanel> tabList = new List<RibbonPanel>();

            tabList = application.GetRibbonPanels(tabName);

            RibbonPanel tab = null;

            foreach (RibbonPanel r in tabList)
            {
                if (r.Name.ToUpper() == panelName.ToUpper())
                {
                    tab = r;
                }
            }

            if (tab is null)
                tab = application.CreateRibbonPanel(tabName, panelName);

            return tab;
        }


        /// <summary>
        /// Add min 2 or max 3 buttons to a stacked button.
        /// </summary>
        private bool AddStackedButton(RibbonPanel panel, IList<PushButtonData> stackedButtonsGroup, string stackedButtonName, string stackedButtonText)
        {
            try
            {
                List<RibbonItem> projectButtons = new List<RibbonItem>();
                projectButtons.AddRange(panel.AddStackedItems(stackedButtonsGroup[0], stackedButtonsGroup[1], stackedButtonsGroup[2]));
            
                return true;
            }
            catch
            {
                return false;
            }            
        }

        /// <summary>
        /// Add min 2 or max 3 buttons to a stacked button.
        /// </summary>
        private bool AddSplitButton(RibbonPanel panel, IList<PushButtonData> splitButtonsGroup, string splitButtonName, string splitButtonText)
        {
            try
            {
                SplitButtonData sb1 = new SplitButtonData(splitButtonName, splitButtonText);
                SplitButton sb = panel.AddItem(sb1) as SplitButton;

                foreach (PushButtonData item in splitButtonsGroup)
                {
                    sb.AddPushButton(item);
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }


        ///<summary>
        ///Add a PushButton to the Ribbon Panel
        ///</summary>
        private PushButtonData CreatePushButton(string ButtonName, string ButtonText, string ImagePath16, string ImagePath32, string dllClass, string Tooltip, string availability="")
        {

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, thisAssemblyPath, dllClass);

                m_pbData.AvailabilityClassName = availability;

                if (ImagePath16 != "")
                {
                    try
                    {
                        m_pbData.Image = new BitmapImage(new Uri(ImagePath16));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }
                if (ImagePath32 != "")
                {
                    try
                    {
                        m_pbData.LargeImage = new BitmapImage(new Uri(ImagePath32));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }

                m_pbData.ToolTip = Tooltip;
                
                return m_pbData;
            }
            catch
            {
                return null;
            }
        }


        ///<summary>
        ///Add a PushButton to the Ribbon Panel
        ///</summary>
        private Boolean AddPushButton(RibbonPanel Panel, string ButtonName, string ButtonText, string ImagePath16, string ImagePath32, string dllClass, string Tooltip)
        {

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, thisAssemblyPath, dllClass);
                
                if (ImagePath16 != "")
                {
                    try
                    {
                        m_pbData.Image = new BitmapImage(new Uri(ImagePath16));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }
                if (ImagePath32 != "")
                {
                    try
                    {
                        m_pbData.LargeImage = new BitmapImage(new Uri(ImagePath32));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }

                m_pbData.ToolTip = Tooltip;


                PushButton m_pb = Panel.AddItem(m_pbData) as PushButton;
                
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Add a push button visible in Revit Zero State mode
        /// </summary>
        /// <param name="Panel"></param>
        /// <param name="ButtonName"></param>
        /// <param name="ButtonText"></param>
        /// <param name="ImagePath16"></param>
        /// <param name="ImagePath32"></param>
        /// <param name="dllClass"></param>
        /// <param name="Tooltip"></param>
        /// <returns></returns>
        private Boolean AddZeroStatePushButton(RibbonPanel Panel, string ButtonName, string ButtonText, string ImagePath16, string ImagePath32, string dllClass, string Tooltip, string Availability)
        {

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, thisAssemblyPath, dllClass);

                m_pbData.AvailabilityClassName = Availability;

                if (ImagePath16 != "")
                {
                    try
                    {
                        m_pbData.Image = new BitmapImage(new Uri(ImagePath16));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }
                if (ImagePath32 != "")
                {
                    try
                    {
                        m_pbData.LargeImage = new BitmapImage(new Uri(ImagePath32));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }

                m_pbData.ToolTip = Tooltip;


                PushButton m_pb = Panel.AddItem(m_pbData) as PushButton;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Add a Text Box to the Ribbon Panel
        /// </summary>
        /// <param name="Panel"></param>
        /// <param name="textboxName"></param>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        private Boolean AddTextBox(RibbonPanel Panel, string textboxName, string tooltip)
        {
            try
            {
                TextBoxData tbData = new TextBoxData(textboxName);
                Autodesk.Revit.UI.TextBox textBox = Panel.AddItem(tbData) as Autodesk.Revit.UI.TextBox;

                textBox.PromptText = "Write something and hit Enter";
                textBox.ShowImageAsButton = true;

                textBox.ToolTip = tooltip;

                textBox.EnterPressed += new EventHandler<TextBoxEnterPressedEventArgs>(MyTextBoxEnter);
                     
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Text Box Event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MyTextBoxEnter(object sender, TextBoxEnterPressedEventArgs args)
        {
            UIDocument uiDoc = args.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            Autodesk.Revit.UI.TextBox textBox = sender as Autodesk.Revit.UI.TextBox;

            string message = textBox.Value.ToString();

            string caseSwitch = "default";

            if (message.Contains("babbo"))
                caseSwitch = "christmas";

            if (message.Contains("leann says"))
                caseSwitch = "leann";

            if (message.Contains("+"))
                caseSwitch = "sum";

            if (message.StartsWith("*"))
                caseSwitch = "select";

            if (message.StartsWith("/"))
                caseSwitch = "selectAll";

            if (message.StartsWith("sheets"))
                caseSwitch = "selectSheets";

            if (message.StartsWith("tblocks"))
                caseSwitch = "selectTBlocks";

            if (message.StartsWith("-"))
                caseSwitch = "delete";

            if (message.StartsWith("+viewset"))
                caseSwitch = "createViewSet";

            switch (caseSwitch)
            {
                case "default":
                    MessageBox.Show(textBox.Value.ToString(), "Command Line");
                    break;
                case "christmas":
                    Helpers.Christams();
                    break;
                case "leann":
                    Helpers.leannSays();
                    break;
                case "sum":
                    Helpers.AddTwoIntegers(message);
                    break;
                case "select":
                    Helpers.SelectAllElementsInView(uiDoc, message);
                    break;
                case "selectAll":
                    Helpers.SelectAllElements(uiDoc, message);
                    break;
                case "createViewSet":
                    Helpers.CreateViewset(doc, message);
                    break;
                case "delete":
                    break;
                case "selectSheets":
                    Helpers.HighlightSelectSheets(uiDoc, message);
                    break;
                case "selectTBlocks":
                    Helpers.HighlightSelectTitleBlocks(uiDoc, message);
                    break;
            }

            
        }

    }
}
