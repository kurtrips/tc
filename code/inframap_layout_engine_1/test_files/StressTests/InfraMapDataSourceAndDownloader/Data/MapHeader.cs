using System;
namespace Astraea.Inframap.Data
{
    /**
     * <p>Represents the header of the MapData. Contains basic header information, such as the lifecycle, version, and the time the parent MapData instance was downloaded. This entity will be created and filled in the MapDataDownloader. This class is Serializable.</p>
     * 
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     * 
     */
    public class MapHeader
    {

        /**
         * <p>Represents life cycle of the file. It will default to 'map_data' in the loader.</p>
         * <p>This can be any value. It will be managed with the Lifecycle property.</p>
         * 
         * 
         */
        private string lifecycle = null;

        /**
         * <p>Represents the version of the map data. It will be '1.0' for now. Set by the loader.</p>
         * <p>This can be any value. It will be managed with the Version property.</p>
         * 
         * 
         */
        private string version = null;

        /**
         * <p>Represents the datetime of downloading. Set by the loader.</p>
         * <p>This can be any value. It will be managed with the Timestamp property.</p>
         * 
         * 
         */
        private DateTime timestamp = DateTime.MinValue;

        /**
         * <p>This is the property for the lifecycle field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the lifecycle field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the lifecycle field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string Lifecycle
        {
            get
            {
                return lifecycle;
            }
            set
            {
                lifecycle = value;
            }
        }

        /**
         * <p>This is the property for the version field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the version field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the version field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }

        /**
         * <p>This is the property for the timestamp field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the timestamp field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the timestamp field to the value.</li>
         * </ul>
         * 
         * 
         */
        public DateTime Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
            }
        }

        /**
         * Default constructor. Does nothing.
         * 
         */
        public MapHeader()
        {
            // empty
        }
    }
}