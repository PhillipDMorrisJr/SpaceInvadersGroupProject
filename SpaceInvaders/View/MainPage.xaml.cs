using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpaceInvaders.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));

            timer = new DispatcherTimer {Interval = gameTickInterval};

            timer.Tick += timerOnTick;
            timer.Start();

            gameManager = new GameManager(ApplicationHeight, ApplicationWidth);
            gameManager.InitializeGame(theCanvas);


            Window.Current.CoreWindow.KeyDown += coreWindowOnKeyDown;
        }

        #endregion

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

        #region Methods

        private void timerOnTick(object sender, object e)
        {
            updateGameStatistics();
        }

        private void displayGameOverScreen()
        {
            if (gameManager.IsGameOver())
            {
                var score = gameManager.GameScore;
                var gameOverDialog = new MessageDialog("Game Over\n" + "Your Final score: " + score);

                gameOverDialog.ShowAsync();
                timer.Stop();
            }
        }

        private void updateGameStatistics()
        {
            textBlock.Text = gameManager.GameStatistics;

            if (gameManager.IsGameOver())
            {
                displayGameOverScreen();
            }
        }

        private async void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    if ((Window.Current.CoreWindow.GetAsyncKeyState(VirtualKey.Space) & CoreVirtualKeyStates.Down) != 0)
                    {
                        gameManager.MovePlayerShipLeft();
                        fireBulltWhenGameIsNotOver();
                    }
                    else
                    {
                        gameManager.MovePlayerShipLeft();
                    }
                    break;


                case VirtualKey.Right:
                    if ((Window.Current.CoreWindow.GetAsyncKeyState(VirtualKey.Space) & CoreVirtualKeyStates.Down) != 0)
                    {
                        gameManager.MovePlayerShipRight();
                        fireBulltWhenGameIsNotOver();
                    }
                    else
                    {
                        gameManager.MovePlayerShipRight();
                    }

                    break;

                case VirtualKey.Space:
                    await playShootSound();

                    if ((Window.Current.CoreWindow.GetAsyncKeyState(VirtualKey.Right) & CoreVirtualKeyStates.Down) != 0)
                    {
                        gameManager.MovePlayerShipRight();
                        fireBulltWhenGameIsNotOver();
                    }
                    else if ((Window.Current.CoreWindow.GetAsyncKeyState(VirtualKey.Left) & CoreVirtualKeyStates.Down) !=
                             0)
                    {
                        gameManager.MovePlayerShipLeft();
                        fireBulltWhenGameIsNotOver();
                    }
                    else
                    {
                        fireBulltWhenGameIsNotOver();
                    }

                    break;
            }
        }

        private static async Task playShootSound()
        {
            var shootSound = new MediaElement();
            var folder =
                await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var file = await folder.GetFileAsync("phasers3.wav");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            shootSound.SetSource(stream, file.ContentType);
            shootSound.Play();
        }

        private void fireBulltWhenGameIsNotOver()
        {
            if (!gameManager.IsGameOver())
            {
                gameManager.FirePlayerBullet();
            }
        }

        #endregion
    }
}