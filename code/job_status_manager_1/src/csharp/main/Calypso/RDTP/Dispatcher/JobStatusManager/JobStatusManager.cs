/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Calypso.RDTP.Entity.Job;
using Calypso.RDTP.Dispatcher.JobStatusManager.Prioritizers;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// This is main class of the component.
    /// It manages collection of jobs and some associated indices.
    /// The prioritizer specifies order to sort jobs.
    /// It exposes functions to add or remove jobs and getting jobs as per different criteria.
    /// </summary>
    ///
    /// <example>
    /// <code>
    /// //Base operations
    ///
    /// // create JobStatusManager
    /// JobStatusManager jobManager = new JobStatusManager();
    ///
    /// // add job
    /// jobManager.AddJob(job1);
    ///
    /// // Remove job
    /// jobManager.RemoveJob(job1.QueueID);
    ///
    /// // add a list of jobs
    /// jobManager.AddJobs(list);
    ///
    /// // Remove a list of jobs
    /// jobManager.RemoveJobs(list);
    ///
    ///
    /// //Get data for various criteria
    ///
    /// // get concrete job (QueueID is unique job identifier)
    /// ITradeActivityItem item1 = jobManager.GetJobByQueueID(2);
    ///
    /// // Get count of jobs with given status
    /// int jobsCount1 = jobManager.GetJobCountByStatus(JobStatus.Queued);
    ///
    /// // get jobs with given status
    /// ReadOnlyCollection&lt;ITradeActivityItem&gt; jobs2 = jobManager.GetJobsByStatus(JobStatus.Queued);
    ///
    /// // get jobs for given service
    /// ReadOnlyCollection&lt;ITradeActivityItem&gt; jobs3 = jobManager.GetJobsByServiceID(3);
    ///
    /// // Get count of jobs with given status for given service
    /// int jobCount2 = jobManager.GetJobCountByStatusServiceID(JobStatus.Queued, 3);
    ///
    /// // get jobs for given service and with given status
    /// ReadOnlyCollection&lt;ITradeActivityItem&gt; jobs4 =
    ///     jobManager.GetJobsByStatusAndServiceID(JobStatus.Queued, 3);
    ///
    /// // get jobs for given service and with JobStatus.Queued status
    /// ReadOnlyCollection&lt;ITradeActivityItem&gt; jobs5 = jobManager.GetJobsForDispatch(3);
    ///
    /// // we can get exclusive access to JobStatusManager to execute not thread safe operations
    /// lock (jobManager.SyncRoot)
    /// {
    ///     foreach (ITradeActivityItem job in jobs4)
    ///     {
    ///         Console.WriteLine(job.QueueID);
    ///      }
    ///  }
    ///
    ///
    /// //Monitor jobs for given criteria
    /// //The application can get read only wrapper for given condition once and uses it many times.
    /// //The wrapper provides read only access to collection of jobs for given criteria.
    ///
    /// // get jobs for dispatch once
    /// ReadOnlyCollection&lt;ITradeActivityItem&gt; jobs = jobManager.GetJobsForDispatch(3);
    ///
    /// // call by timer for example
    /// foreach (ITradeActivityItem job in jobs)
    /// {
    ///     // process job
    /// }
    /// </code>
    /// </example>
    ///
    /// <threadsafety>
    /// The class is mutable ( application can add / remove jobs) but it is thread safe as it uses syncRoot
    /// to provide exclusive access to collections.
    /// </threadsafety>
    ///
    /// <author>dfn</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class JobStatusManager
    {
        /// <summary>
        /// <para>
        /// The index to search job by queueID. The element key is job queueID, the value is appropriate job. The
        /// variable can not be null and contains elements with null values. This variable is immutable but its
        /// content can be changed  by AddJob / AddJobs / RemoveJob / RemoveJobs methods. </para>
        /// </summary>
        private readonly IDictionary<long, ITradeActivityItem> jobsByQueueID =
            new Dictionary<long, ITradeActivityItem>();

        /// <summary>
        /// <para> Index to search job by status. The element key is job integer value of <see cref="JobStatus"/>,
        /// he value is collection of jobs with given status.
        /// The variable can not be null and contains elements with null values.
        /// This variable is immutable but its content can be changed  by AddJob / AddJobs / RemoveJob / RemoveJobs
        /// methods. </para>
        /// </summary>
        private readonly IDictionary<int, JobSortedCollection> jobsByStatus =
            new Dictionary<int, JobSortedCollection>();

        /// <summary>
        /// <para> Index to search job by serviceID. The element key is job service ID, the value is collection of jobs
        /// with given service ID. The variable can not be null and contains elements with null values. This variable is
        /// immutable but its content can be changed  by AddJob / AddJobs / RemoveJob / RemoveJobs methods. </para>
        /// </summary>
        private readonly IDictionary<long, JobSortedCollection> jobsByServiceId =
            new Dictionary<long, JobSortedCollection>();

        /// <summary>
        /// <para> Index to search job by status and service ID. The element key is <see cref="JobInfo"/> instance,
        /// the value is collection of jobs with given status and service ID.
        /// The variable can not be null and contains elements with null values.
        /// This variable is immutable but its content can be changed  by AddJob / AddJobs / RemoveJob /
        /// RemoveJobs methods. </para>
        /// </summary>
        private readonly IDictionary<JobInfo, JobSortedCollection> jobsByStatusAndServiceID =
            new Dictionary<JobInfo, JobSortedCollection>();

        /// <summary>
        /// <para><see cref="ITradeActivityItem"/> comparer used to sort jobs.
        /// This variable is set in constructor and never be changed after it.
        /// It is immutable, it will never be null. </para>
        /// </summary>
        private readonly IPrioritizer jobPrioritizer;

        /// <summary>
        /// <para>
        /// Object used to synchronize access to internal collections. It is immutable, it will never be null.
        /// </para>
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// <para>Gets an object for synchronizing across multiple call operations.
        /// The application can use this property to guarantee thread safety for multiple calls operations.</para>
        /// </summary>
        /// <value>An object for synchronizing across multiple call operations.</value>
        public object SyncRoot
        {
            get
            {
                return syncRoot;
            }
        }

        /// <summary>
        /// <para>Constructs class instance with default prioritizer.</para>
        /// </summary>
        public JobStatusManager() : this(new DefaultPrioritizer())
        {
        }

        /// <summary>
        /// <para> Constructs class instance with given prioritizer.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">if prioritizer is null</exception>
        /// <param name="prioritizer">prioritizer object to sort jobs</param>
        public JobStatusManager(IPrioritizer prioritizer)
        {
            Helper.ValidateNotNull(prioritizer, "prioritizer");
            jobPrioritizer = prioritizer;
        }

        /// <summary>
        /// <para>Adds a job to collection.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">if parameter is null</exception>
        /// <exception cref="ArgumentException">
        /// If job.Status is not valid <see cref="JobStatus"/> value OR
        /// If a job with given queueId has already been added to the collection.
        /// </exception>
        /// <param name="job">job to add</param>
        public void AddJob(ITradeActivityItem job)
        {
            //Validate
            Helper.ValidateNotNull(job, "job");
            Helper.ValidateJobStatus(job.Status, "job.Status");

            lock (syncRoot)
            {
                //Add to jobsByQueueID
                //NOTE - Must check by queueId and not by job
                if (jobsByQueueID.ContainsKey(job.QueueID))
                {
                    throw new ArgumentException("Job with queueId: " + job.QueueID + " has already been added.", "job");
                }
                jobsByQueueID[job.QueueID] = job;

                //Add to jobsByStatus
                AddTradeActivityItemToIndex<int>(jobsByStatus, (int)job.Status, job);

                //Add to jobsByServiceId
                AddTradeActivityItemToIndex<long>(jobsByServiceId, job.ServiceID, job);

                //Add to jobsByStatusAndServiceID
                JobInfo jobInfo = new JobInfo(job.ServiceID, job.Status);
                AddTradeActivityItemToIndex<JobInfo>(jobsByStatusAndServiceID, jobInfo, job);

                //Subsribes to job.StatusChanged event
                job.StatusChanged += new EventHandler<StatusEventArgs>(HandleStatusChange);
            }
        }

        /// <summary>
        /// <para>Adds jobs to collection.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">if jobs is null.</exception>
        /// <exception cref="ArgumentException">
        /// If jobs contains null elements. OR
        /// If a job has a status which is not valid <see cref="JobStatus"/> value OR
        /// If a job with given queueId has already been added to the collection.
        /// </exception>
        /// <param name="jobs">list of jobs to add.</param>
        public void AddJobs(IEnumerable<ITradeActivityItem> jobs)
        {
            //Check for null and null elements.
            Helper.ValidateJobCollection(jobs, "jobs");

            Collection<ITradeActivityItem> cache = new Collection<ITradeActivityItem>();
            try
            {
                foreach (ITradeActivityItem job in jobs)
                {
                    AddJob(job);

                    // cache the job successfully added
                    cache.Add(job);
                }
            }
            catch (ArgumentException e)
            {
                // failed to add all jobs
                // removed jobs that have been added
                RemoveJobs(cache);

                // rethrow the exception
                throw e;
            }
        }

        /// <summary>
        /// <para>
        /// Gets job with given Queue ID. Returns null if collection doesn't contain job with given Queue ID.
        /// </para>
        /// </summary>
        /// <param name="queueID">queue ID to find job for</param>
        /// <returns>job with given Queue ID or null if collection doesn't contain job with given Queue ID.</returns>
        public ITradeActivityItem GetJobByQueueID(long queueID)
        {
            ITradeActivityItem ret;
            lock (syncRoot)
            {
                jobsByQueueID.TryGetValue(queueID, out ret);
                return ret;
            }
        }

        /// <summary>
        /// <para>Gets count of jobs with given status.</para>
        /// </summary>
        /// <exception cref="ArgumentException">if status is not valid <see cref="JobStatus"/> value</exception>
        /// <param name="status">job status</param>
        /// <returns>count of jobs with given status</returns>
        public int GetJobCountByStatus(JobStatus status)
        {
            return GetJobsByStatus(status).Count;
        }

        /// <summary>
        /// <para>Gets count of jobs with given status and service ID.</para>
        /// </summary>
        /// <param name="status">job status</param>
        /// <param name="serviceID">service ID of job</param>
        /// <returns>count of jobs with given parameters</returns>
        /// <exception cref="ArgumentException">If status is not a valid <see cref="JobStatus"/> enum value.</exception>
        public int GetJobCountByStatusServiceID(JobStatus status, long serviceID)
        {
            return GetJobsByStatusAndServiceID(status, serviceID).Count;
        }

        /// <summary>
        /// <para>Gets list of jobs with given service ID. Returns empty collection if manager doesn't contain job with
        /// given status </para>
        /// </summary>
        /// <param name="serviceID">job service ID</param>
        /// <returns>collection of jobs with given service ID</returns>
        public ReadOnlyCollection<ITradeActivityItem> GetJobsByServiceID(long serviceID)
        {
            JobSortedCollection jobs;
            lock (syncRoot)
            {
                //Create a new JobSortedCollection if none was found
                if (!jobsByServiceId.TryGetValue(serviceID, out jobs))
                {
                    jobs = new JobSortedCollection(new DefaultPrioritizer());
                    jobsByServiceId[serviceID] = jobs;
                }
                return jobs;
            }
        }

        /// <summary>
        /// <para>Gets list of jobs with given status.
        /// Returns empty collection if manager doesn't contain job with given status.</para>
        /// </summary>
        /// <exception cref="ArgumentException">if status is not valid <see cref="JobStatus"/> value</exception>
        /// <param name="status">job status</param>
        /// <returns>collection of jobs with given status</returns>
        public ReadOnlyCollection<ITradeActivityItem> GetJobsByStatus(JobStatus status)
        {
            Helper.ValidateJobStatus(status, "status");

            JobSortedCollection jobs;
            lock (syncRoot)
            {
                jobsByStatus.TryGetValue((int)status, out jobs);

                //Create a new JobSortedCollection if none was found
                if (jobs == null)
                {
                    jobs = new JobSortedCollection(new DefaultPrioritizer());
                    jobsByStatus[(int)status] = jobs;
                }
                return jobs;
            }
        }

        /// <summary>
        /// <para>Gets list of jobs with given status and service ID.
        /// Returns empty collection if manager doesn't contain job with given status and service ID </para>
        /// </summary>
        /// <exception cref="ArgumentException">if status is not valid <see cref="JobStatus"/> value</exception>
        /// <param name="status">job status</param>
        /// <param name="serviceID">job service ID</param>
        /// <returns>collection of job with given parameters</returns>
        public ReadOnlyCollection<ITradeActivityItem> GetJobsByStatusAndServiceID(JobStatus status, long serviceID)
        {
            JobInfo jobInfo = new JobInfo(serviceID, status);

            JobSortedCollection jobs;
            lock (syncRoot)
            {
                jobsByStatusAndServiceID.TryGetValue(jobInfo, out jobs);

                //Create a new JobSortedCollection if none was found
                if (jobs == null)
                {
                    jobs = new JobSortedCollection(new DefaultPrioritizer());
                    jobsByStatusAndServiceID[jobInfo] = jobs;
                }
                return jobs;
            }
        }

        /// <summary>
        /// <para>Gets jobs with status queued and the given serviceId</para>
        /// </summary>
        /// <param name="serviceID">job service ID</param>
        /// <returns>collection of jobs with status queued and the given serviceId</returns>
        public ReadOnlyCollection<ITradeActivityItem> GetJobsForDispatch(long serviceID)
        {
            return GetJobsByStatusAndServiceID(JobStatus.Queued, serviceID);
        }

        /// <summary>
        /// <para>
        /// Removes job with given queueId from collection and returns it. If the job was not found, returns null.
        /// </para>
        /// </summary>
        /// <param name="queueID">job Queue ID</param>
        /// <returns>The removed job if job with given queueId was found or null if no job was found.</returns>
        public ITradeActivityItem RemoveJob(long queueID)
        {
            ITradeActivityItem jobToRemove;
            lock (syncRoot)
            {
                jobsByQueueID.TryGetValue(queueID, out jobToRemove);

                //No job for given queueId found
                if (jobToRemove == null)
                {
                    return null;
                }

                //Remove from all indices
                jobsByQueueID.Remove(queueID);
                jobsByServiceId[jobToRemove.ServiceID].Remove(jobToRemove);
                jobsByStatus[(int)jobToRemove.Status].Remove(jobToRemove);
                jobsByStatusAndServiceID[new JobInfo(jobToRemove.ServiceID, jobToRemove.Status)].Remove(jobToRemove);

                //Unsubscribes to job.StatusChanged event
                jobToRemove.StatusChanged -= new EventHandler<StatusEventArgs>(HandleStatusChange);
            }

            return jobToRemove;
        }

        /// <summary>
        /// <para>Removes jobs from collection.</para>
        /// </summary>
        /// <param name="jobs">list of jobs to remove</param>
        public void RemoveJobs(IEnumerable<ITradeActivityItem> jobs)
        {
            Helper.ValidateJobCollection(jobs, "jobs");

            //Delegate each job to RemoveJob
            foreach (ITradeActivityItem job in jobs)
            {
                RemoveJob(job.QueueID);
            }
        }

        /// <summary>
        /// <para>
        /// Event Handler for Job Status Changed event.
        /// The event is raised by the SetStatus methods of <see cref="ITradeActivityItem"/> and is handled
        /// here to maintain the internal indices of the <see cref="JobStatusManager"/>.
        /// </para>
        /// </summary>
        /// <param name="sender">event source</param>
        /// <param name="e">event data</param>
        /// <exception cref="ArgumentNullException">If any parameter is null.</exception>
        /// <exception cref="ArgumentException">
        /// If sender is not of type <see cref="ITradeActivityItem"/>.
        /// </exception>
        protected virtual void HandleStatusChange(object sender, StatusEventArgs e)
        {
            //Validate
            Helper.ValidateNotNull(sender, "sender");
            Helper.ValidateNotNull(e, "e");
            if (!(sender is ITradeActivityItem))
            {
                throw new ArgumentException("sender must be of type ITradeActivityItem.", "sender");
            }

            ITradeActivityItem job = sender as ITradeActivityItem;

            lock (syncRoot)
            {
                //Remove job from status index. Note that removing from other indices is useless.
                if (jobsByStatus.ContainsKey((int)e.OldStatus))
                {
                    jobsByStatus[(int)e.OldStatus].Remove(job);
                }

                //Add job to status index with the new status. Note that adding to other indices is useless.
                AddTradeActivityItemToIndex<int>(jobsByStatus, (int)e.NewStatus, job);
            }
        }

        /// <summary>
        /// Adds an <see cref="ITradeActivityItem"/> instance (job) to the given dictionary.
        /// If the key to be found is not found in the dictionary,
        /// then a new entry (<see cref="JobSortedCollection"/> instance) is added to the dictionary and the job is
        /// added to the new <see cref="JobSortedCollection"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of key of the dictionary.</typeparam>
        /// <param name="index">The dictionary in which to add.</param>
        /// <param name="keyToFind">The key which is supposed to be found in the dictionary.</param>
        /// <param name="jobToAdd">The job that needs to be added to the dictionary.</param>
        private static void AddTradeActivityItemToIndex<T>(IDictionary<T, JobSortedCollection> index, T keyToFind,
            ITradeActivityItem jobToAdd)
        {
            //Check if the key is already present in the index
            JobSortedCollection coll;
            index.TryGetValue(keyToFind, out coll);

            if (coll == null)
            {
                //Create new JobSortedCollection
                coll = new JobSortedCollection(new DefaultPrioritizer());

                //Add the new JobSortedCollection to the dictionary
                index[keyToFind] = coll;
            }

            //Add the job to the created or found JobSortedCollection
            coll.Add(jobToAdd);
        }
    }
}
