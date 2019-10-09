/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Calypso.RDTP.Entity.Job;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// This collection stores <see cref="ITradeActivityItem"/> instances sorted in order specified by comparer.
    /// The class implements ICollection&lt;<see cref="ITradeActivityItem"/>&gt; interface.
    /// </summary>
    ///
    /// <threadsafety>
    /// The class is mutable and not thread safe.
    /// However access to it by <see cref="JobStatusManager"/> class is done in a thread safe manner.
    /// </threadsafety>
    ///
    /// <author>dfn</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class JobSortedCollection : ICollection<ITradeActivityItem>
    {
        /// <summary>
        /// <para>Comparer used to sort items. The variable is set by constructor and will never be changed after it.
        /// It is immutable, it will never be null. </para>
        /// </summary>
        private readonly JobComparer comparer;

        /// <summary>
        /// <para>List of items. Elements of list should be non-null. The variable is immutable, but its content can be
        /// changed by Add and Remove methods. This variable will never be null. </para>
        /// </summary>
        private readonly List<ITradeActivityItem> items = new List<ITradeActivityItem>();

        /// <summary>
        /// <para>Count of items in collection.</para>
        /// </summary>
        /// <value>Number of items in collection.</value>
        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        /// <summary>
        /// <para>Indicates whether this collection is read only.</para>
        /// </summary>
        /// <value>Always false</value>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// <para>Constructs class instance with given prioritizer. Creates JobComparer for given prioritizer.</para>
        /// </summary>
        /// <param name="prioritizer">comparer to order items</param>
        /// <exception cref="ArgumentNullException">if prioritizer is null</exception>
        public JobSortedCollection(IPrioritizer prioritizer)
        {
            comparer = new JobComparer(prioritizer);
        }

        /// <summary>
        /// <para>
        /// Adds an item to collection.
        /// The adding is done at the appropriate index as returned by a search based on the <see cref="IPrioritizer"/>.
        /// </para>
        /// </summary>
        /// <param name="item">item to add</param>
        /// <exception cref="ArgumentNullException">if item is null</exception>
        /// <exception cref="ArgumentException">if Status of item is invalid <see cref="JobStatus"/> enum.</exception>
        public void Add(ITradeActivityItem item)
        {
            Helper.ValidateNotNull(item, "item");
            Helper.ValidateJobStatus(item.Status, "item.Status");

            //Look for the appropriate place to insert.
            int posToInsert = items.BinarySearch(item, comparer);
            if (posToInsert < 0)
            {
                posToInsert = ~posToInsert;
            }

            items.Insert(posToInsert, item);
        }

        /// <summary>
        /// <para>Clears the collection.</para>
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// <para>
        /// Indicates whether this collection contains given element.
        /// Two items are considered equal based on their comparison done by
        /// the <see cref="IPrioritizer"/> used by this class.
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException">if item is null.</exception>
        /// <param name="item">item to check</param>
        /// <returns>true if collection given item, false otherwise</returns>
        public bool Contains(ITradeActivityItem item)
        {
            Helper.ValidateNotNull(item, "item");
            return items.BinarySearch(item, comparer) >= 0;
        }

        /// <summary>
        /// <para>
        /// Copies items to given array started from given index.
        /// This method propagates all exceptions.
        /// </para>
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        /// If array is a null reference.
        /// </exception>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        /// index is less than 0.
        /// </exception>
        ///
        /// <exception cref="ArgumentException">
        /// index is equal to or greater than the length of array,
        /// or the number of elements in the source collection is greater than
        /// the available space from index to the end of the destination array.
        /// </exception>
        ///
        /// <param name="array">array to copy to</param>
        /// <param name="index">starting index</param>
        public void CopyTo(ITradeActivityItem[] array, int index)
        {
            items.CopyTo(array, index);
        }

        /// <summary>
        /// <para>Removes given item from collection.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">if item is null</exception>
        /// <param name="item">item to remove</param>
        /// <returns>true if item is removed, false otherwise</returns>
        public bool Remove(ITradeActivityItem item)
        {
            Helper.ValidateNotNull(item, "item");

            //Search for item
            int posToRemove = items.BinarySearch(item, comparer);

            //Return false if not found.
            if (posToRemove < 0)
            {
                return false;
            }
            //Otherwise remove the item found and return false.
            else
            {
                items.RemoveAt(posToRemove);
                return true;
            }
        }

        /// <summary>
        /// <para>Gets generic enumerator for the collection.</para>
        /// </summary>
        /// <returns>Generic enumerator for the collection.</returns>
        public IEnumerator<ITradeActivityItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// <para>Gets non-generic enumerator for collection.</para>
        /// </summary>
        /// <returns>Non-generic enumerator for collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// <para>
        /// Implicitly converts the collection to ReadOnlyCollection&lt;<see cref="ITradeActivityItem"/>&gt; type.
        /// If the collection is null then null is returned
        /// but of the ReadOnlyCollection&lt;<see cref="ITradeActivityItem"/>&gt; type.
        /// </para>
        /// </summary>
        /// <param name="collection">sorted collection to convert</param>
        /// <returns>ReadOnlyCollection instance for given collection</returns>
        public static implicit operator ReadOnlyCollection<ITradeActivityItem>(JobSortedCollection collection)
        {
            //If null return null but with type ReadOnlyCollection<ITradeActivityItem>
            if (collection == null)
            {
                return (ReadOnlyCollection<ITradeActivityItem>)null;
            }
            else
            {
                return new ReadOnlyCollection<ITradeActivityItem>(collection.items);
            }
        }

        /// <summary>
        /// This comparer adds job Queue ID as last comparison criteria.
        /// It guarantees that there is the only element with given sorting key.
        /// </summary>
        /// <threadsafety>
        /// This class is immutable and thread safe
        /// </threadsafety>
        /// <author>dfn</author>
        /// <author>TCSDEVELOPER</author>
        /// <version>1.0</version>
        /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
        private class JobComparer : IPrioritizer
        {
            /// <summary>
            /// <para>Prioritizer to order jobs. This variable is set by constructor and will never be changed after it.
            /// This variable is immutable and will never be null. </para>
            /// </summary>
            private readonly IPrioritizer prioritizer;

            /// <summary>
            /// <para>Constructs class instance with given parameters </para>
            /// </summary>
            /// <exception cref="ArgumentNullException">if prioritizer is null</exception>
            /// <param name="prioritizer">The prioritizer instance to use for comparing.</param>
            public JobComparer(IPrioritizer prioritizer)
            {
                Helper.ValidateNotNull(prioritizer, "prioritizer");
                this.prioritizer = prioritizer;
            }

            /// <summary>
            /// <para>Compares two <see cref="ITradeActivityItem"/> instances</para>
            /// <para>Compares jobs using prioritizer. If they are same, then compares them as per their QueueIds.
            /// </para>
            /// </summary>
            /// <param name="job">job to compare</param>
            /// <param name="otherJob">other job to compare</param>
            /// <returns>
            /// negative number if job is less than otherJob,
            /// zero if they are equal,
            /// positive number if job is greater than otherJob
            /// </returns>
            public int Compare(ITradeActivityItem job, ITradeActivityItem otherJob)
            {
                //Use prioritizer to compare first
                int comp = prioritizer.Compare(job, otherJob);
                if (comp != 0)
                {
                    return comp;
                }

                //Comapre the queueIds
                return job.QueueID.CompareTo(otherJob.QueueID);
            }
        }

    }
}
