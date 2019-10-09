
using System.Collections.Generic;
namespace TopCoder.Graph.Layout
{

    /**
     * <p>
     * The instance of this class describe a segment connecting a series of coordinates, this is the data structure used to
     * describe a link's path.(Several Segment instances together describe a single link)
     * </p>
     * <p>
     * Thread safety: This class is not thread safe since the path field is mutable and no synchronizations are performed
     * for the accesses to it.
     * </p>
     * 
     * 
     */
    public class Segment
    {

        /**
         * <p>
         * This field represents the path of this segment. The segment is formed by connecting the coordinates in this list
         * one after another.
         * It's initialized in the constructors and can be gotten through the getter and modified through the setter so It's mutable.
         * It will never be null, but it can be an empty list in which case the segment is just empty. Every elements in the list
         * are non-null Coordinates instances. Usually the coordinates in the list will not be duplicate(both the reference and
         * the value) but that's not a hard requirement.
         * </p>
         * 
         * 
         */
        private IList<Coordinates> path;

        /**
         * <p>
         * This is the property for the path field.
         * The getter returns a read-only IList wrapper for the path field. The return value wil never be null, but it can be
         * empty if the segment is empty. Every elements in the list are not null. Usually the coordinates in the list will not be
         * duplicate(both the reference and the value) but that's not a hard requirement.
         * The setter sets the field with a shallow copy for the value passed in after performing checkings.
         * </p>
         * 
         * 
         * 
         * @throws ArgumentException If the list to set contains null elements.
         * @throws ArgumentNullException If we set null to this field via the setter.
         */
        public IList<Coordinates> Path
        {
            get
            {
                return path;
            }
            set
            {
                this.path = value;
            }
        }

        /**
         * <p>
         * This constructor creates an empty Segment instance.
         * Simply delegates to this(new List<Coordinates>()).
         * </p>
         * 
         * 
         */
        public Segment()
            : this(new List<Coordinates>())
        {
        }

        /**
         * <p>
         * This constructor creates a Segment instance with specified path.
         * Simply sets the argument to the associated field using the setter property.
         * </p>
         * 
         * 
         * 
         * @param path the path of this segment. Can't be null but can be empty, can't contain null elements.
         * @throws ArgumentException If the path argument contains null elements.
         * @throws ArgumentNullException If the path argument is null.
         */
        public Segment(IList<Coordinates> path)
        {
            this.path = path;
        }

        /**
         * <p>
         * Add a coordinates to this segment.
         * Simply add the coordinate to the path list.
         * </p>
         * 
         * 
         * 
         * @param coordinates The coordinates to add. It can't be null.
         * @throws ArgumentNullException If the coordinates is null.
         */
        public void AddCoordinates(TopCoder.Graph.Layout.Coordinates coordinates)
        {
            path.Add(coordinates);
        }

        /**
         * <p>
         * Insert a coordinates to this segment at the specified position.
         * Simply insert the coordinate to the specified position of the path list.
         * Simply delegate to the Insert method of path list.
         * </p>
         * 
         * 
         * 
         * @param index The index for the coordinates to insert. It must be in [0, path.Count-1]
         * @param coordinates The coordinates to insert. It can't be null.
         * @throws ArgumentNullException If the coordinates is null.
         * @throws ArgumentOutOfRangeException If index is not in [0, path.Count-1]
         */
        public void InsertCoordinates(int index, TopCoder.Graph.Layout.Coordinates coordinates)
        {
            path.Insert(index, coordinates);
        }

        /**
         * <p>
         * Remove a coordinates from this segment.
         * It will remove the first occurrence of coordinates param from the path list. If it doesn't exist in the list, nothing will be done.
         * Simply delegate to the Remove method of path list.
         * </p>
         * 
         * 
         * 
         * @param coordinates The coordinates to remove. It can't be null.
         * @return true if the coordinates is successfully removed; otherwise, false. This method also returns false if the param is not found in the path list. 
         * @throws ArgumentNullException If the coordinates param is null.
         */
        public bool RemoveCoordinates(TopCoder.Graph.Layout.Coordinates coordinates)
        {
            return path.Remove(coordinates);
        }

        /**
         * <p>
         * Remove a coordinate at the specific position from this segment. 
         * Simply delegate to the RemoveAt method of path list.
         * </p>
         * 
         * 
         * 
         * @param index The index for the coordinates to remove. It must be in [0, path.Count-1]
         * @throws ArgumentOutOfRangeException If index is not in [0, path.Count-1]
         */
        public void RemoveCoordinatesAt(int index)
        {
            path.RemoveAt(index);
        }

        /**
         * <p>
         * Checks whether a coordinates is in the path of this segment.
         * Simply checks the path list that whether the argument is contained.
         * </p>
         * 
         * 
         * 
         * @param coordinates The coordinates to check. Can't be null.
         * @return whether the coordinates is in the path of this segment.
         * @throws ArgumentNullException If the coordinates param is null.
         */
        public bool ContainsCoordinates(TopCoder.Graph.Layout.Coordinates coordinates)
        {
            return path.Contains(coordinates); ;
        }

        /**
         * <p>
         * Clears all the coordinates of this segment, the segment becomes empty after this operation.
         * Simply clears the path list.
         * </p>
         * 
         * 
         */
        public void ClearAllCoordinates()
        {
            path.Clear();
        }
    }
}