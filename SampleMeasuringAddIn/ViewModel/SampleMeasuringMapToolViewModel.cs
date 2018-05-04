using System.Threading;
using GeoAPI.Geometries;
using GEONIS.Core.Logging;
using GEONIS.gear.AddIn.MapTools;
using GEONIS.Runtime.Core;
using GEONIS.Runtime.Core.Features;
using GEONIS.Runtime.Core.Messages;
using GEONIS.Ui.Wpf.ApplicationFramework.Messages;
using SampleMeasuringAddIn.Services;

namespace SampleMeasuringAddIn.ViewModel
{
    /// <summary>
    /// ViewModel class for the SampleMeasuringMapToolViewModel MapTool.
    /// </summary>
    public class SampleMeasuringMapToolViewModel : GToolViewModelBase
    {

        #region Constants and Fields

        private readonly SampleMeasuringDrawingService _drawingService;
        #endregion

        #region Fields

        /// <summary>
        /// The associated map
        /// </summary>
        private IGMapModel _associatedMap;

        #endregion

        #region Constructor

        public SampleMeasuringMapToolViewModel()
        {
            // for design mode
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleMeasuringMapToolViewModel" /> class.
        /// </summary>
        public SampleMeasuringMapToolViewModel(SampleMeasuringDrawingService drawingService,
            IMessageContext messageContext, IGLogger logger)
            : base(messageContext, logger)
        {
            _drawingService = drawingService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Activates this instance.
        /// </summary>
        public override void Activate()
        {
            if (IsActive)
            {
                return;
            }

            Messenger.Send(new ClearMapHighlightGraphicsMessage());
            IsActive = true;
        }

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        public override void Deactivate()
        {
            if (!IsActive)
            {
                return;
            }

            _drawingService.Clear();
            IsActive = false;
        }

        /// <summary>
        /// Registers the map.
        /// </summary>
        /// <param name="map">The map.</param>
        public void RegisterMap(IGMapModel map)
        {
            _associatedMap = map;
        }

        /// <summary>
        /// Called when [map clicked].
        /// </summary>
        /// <param name="point">The point.</param>
        public void OnMapClicked(IPoint point)
        {
            //Todo: add code handling a click on the map

            _drawingService.AddVertex(point);
        }

        /// <inheritDoc />
        public bool SupportsObject(GeomObject geomObject)
        {
            //Todo: add any code, if the tool supports only specific objects

            return true;
        }

        #endregion
    }
}