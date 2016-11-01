using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model;
using SpaceInvaders.Util;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpaceInvaders.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        /// <summary>
        ///     The application height
        /// </summary>
        public const double ApplicationHeight = 720;

        /// <summary>
        ///     The application width
        /// </summary>
        public const double ApplicationWidth = 1080;

        /// <summary>
        ///     The tick interval in milliseconds
        /// </summary>
        public const int TickInterval = 25;

        private readonly TimeSpan gameTickInterval = new TimeSpan(0, 0, 0, 0, TickInterval);
        private readonly DispatcherTimer timer;

        private readonly GameManager gameManager;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));

            this.timer = new DispatcherTimer {Interval = this.gameTickInterval};

            this.timer.Tick += this.timerOnTick;
            this.timer.Start();

            this.gameManager = new GameManager(ApplicationHeight, ApplicationWidth);
            this.gameManager.InitializeGame(this.theCanvas);


                Window.Current.CoreWindow.KeyDown += this.coreWindowOnKeyDown;
            

        }

        #endregion

        #region Methods

        private void timerOnTick(object sender, object e)
        {
            this.updateGameStatistics();
        }

        private void displayGameOverScreen()
        {
            if (this.gameManager.IsGameOver())
            {
                var score = this.gameManager.GameScore;
                var gameOverDialog = new MessageDialog("Game Over\n" + "Your Final score: " + score);

                gameOverDialog.ShowAsync();
                this.timer.Stop();
            }
        }

        private void updateGameStatistics()
        {
            this.textBlock.Text = this.gameManager.GameStatistics;

            if (this.gameManager.IsGameOver())
            {
                this.displayGameOverScreen();
            }
        }

        private async void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    if ((Window.Current.CoreWindow.GetAsyncKeyState(VirtualKey.Space) & CoreVirtualKeyStates.Down) != 0)
                    {
                        this.gameManager.MovePlayerShipLeft();
                        this.fireBulltWhenGameIsNotOver();
                    }
                    else
                    {
                        this.gameManager.MovePlayerShipLeft();
                    }
                    break;


                case VirtualKey.Right:
                    if ((Window.Current.CoreWindow.GetAsyncKeyState(VirtualKey.Space) & CoreVirtualKeyStates.Down) != 0)
                    {
                        this.gameManager.MovePlayerShipRight();
                        this.fireBulltWhenGameIsNotOver();
                    }
                    else
                    {
                        this.gameManager.MovePlayerShipRight();
                    }

                    break;

                case VirtualKey.Space:
                    //   SoundFx.PlaySound();
                    await playShootSound();

                    if ((Window.Current.CoreWindow.GetAsyncKeyState(VirtualKey.Right) & CoreVirtualKeyStates.Down) != 0)
                    {
                        this.gameManager.MovePlayerShipRight();
                        this.fireBulltWhenGameIsNotOver();
                    }
                    else if ((Window.Current.CoreWindow.GetAsyncKeyState(VirtualKey.Left) & CoreVirtualKeyStates.Down) !=
                             0)
                    {
                        this.gameManager.MovePlayerShipLeft();
                        this.fireBulltWhenGameIsNotOver();
                    }
                    else
                    {
                        this.fireBulltWhenGameIsNotOver();
                    }

                    break;
            }
        }

        private static async Task playShootSound()
        {
            MediaElement mysong = new MediaElement();
            Windows.Storage.StorageFolder folder =
            await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("phasers3.wav");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            mysong.SetSource(stream, file.ContentType);
            mysong.Play();
        }

        private void fireBulltWhenGameIsNotOver()
        {
            if (!this.gameManager.IsGameOver())
            {
                this.gameManager.FirePlayerBullet();
            }
        }

        #endregion
    }
}