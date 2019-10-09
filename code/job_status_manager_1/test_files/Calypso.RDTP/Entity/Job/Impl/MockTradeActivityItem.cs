// MockTradeActivityItem.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace Calypso.RDTP.Entity.Job.Impl
{
    /// <summary>
    /// <para>This is a mock implementation of ITradeActivityItem.</para>
    /// </summary>
    /// <threadsafety>This class is not thread safe as it it mutable.</threadsafety>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MockTradeActivityItem : ITradeActivityItem
    {
        /// <summary>
        /// Accounting treatment. Set in the constructor, doesn't change. Might be empty, never null. Accessed through
        /// corresponding getter.
        /// </summary>
        private readonly string accountingTreatment;

        /// <summary>
        /// Creation timestamp. Set in the constructor, doesn't change. Can be any DateTime. Accessed through
        /// corresponding getter.
        /// </summary>
        private readonly DateTime creationDate;

        /// <summary>
        /// Job priority. Set in the constructor, can also be set through SetJobPriority, which also updates
        /// persistence. Will be non-negative. Accessed through corresponding getter.
        /// </summary>
        private int jobPriority;

        /// <summary>
        /// Primary key shared between TAI and TRADE_QUEUE. Set in the constructor, doesn't change. Will be positive.
        /// Accessed through corresponding getter.
        /// </summary>
        private readonly long queueID;

        /// <summary>
        /// Service ID. Set in the constructor, changed through setServiceID. Will be positive. Accessed through
        /// corresponding getter.
        /// </summary>
        private long serviceID;

        /// <summary>
        /// Job Status. Set in the constructor, changed through the SetStatus methods. Can be any defined member of
        /// JobStatus. Accessed through corresponding getter.
        /// </summary>
        private JobStatus status;

        /// <summary>
        /// Getter for accounting treatment.
        /// </summary>
        /// <value>Gets the accounting treatment.</value>
        public string AccountingTreatment
        {
            get
            {
                return accountingTreatment;
            }
        }

        /// <summary>
        /// getter for creation timestamp.
        /// </summary>
        /// <value>Gets the creation timestamp.</value>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
        }

        /// <summary>
        /// Getter for job priority.
        /// </summary>
        /// <value>Gets the job priority.</value>
        public int JobPriority
        {
            get
            {
                return jobPriority;
            }
        }

        /// <summary>
        /// Getter for primary key shared between TAI and TRADE_QUEUE.
        /// </summary>
        /// <value>Gets the queue id</value>
        public long QueueID
        {
            get
            {
                return queueID;
            }
        }

        /// <summary>
        /// Getter for service ID.
        /// </summary>
        /// <value>Gets the service id</value>
        public long ServiceID
        {
            get
            {
                return serviceID;
            }
        }

        /// <summary>
        /// Getter for job Status.
        /// </summary>
        /// <value>Gets the job Status.</value>
        public JobStatus Status
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// Event for status change. Raised by the SetStatus methods.
        /// </summary>
        public event EventHandler<StatusEventArgs> StatusChanged;

        /// <summary>
        /// This constructor sets the fields to the given values.
        /// </summary>
        /// <param name="accountingTreatment">accounting treatment</param>
        /// <param name="creationDate">creation date</param>
        /// <param name="jobPriority">priority</param>
        /// <param name="queueID">item id</param>
        /// <param name="serviceID">service id</param>
        /// <param name="status">status</param>
        public MockTradeActivityItem(string accountingTreatment, DateTime creationDate, int jobPriority,
            long queueID, long serviceID, JobStatus status)
        {
            this.accountingTreatment = accountingTreatment;
            this.creationDate = creationDate;
            this.jobPriority = jobPriority;
            this.queueID = queueID;
            this.serviceID = serviceID;
            this.status = status;
        }

        /// <summary>
        /// Sets status code and raises the StatusChanged event.
        /// </summary>
        /// <param name="statusCode">new status</param>
        public void SetStatus(JobStatus statusCode)
        {
            JobStatus oldStatus = this.status;

            //Change status
            this.status = statusCode;

            //Raise event
            StatusChanged(this, new StatusEventArgs(oldStatus, statusCode, string.Empty, DateTime.Today));
        }

        /// <summary>
        /// A mock method which raises the StatusChanged with sender as null
        /// </summary>
        public void SetStatusFail1()
        {
            //Raise event with sender as null
            StatusChanged(null, new StatusEventArgs(this.status, this.status, string.Empty, DateTime.Today));
        }

        /// <summary>
        /// A mock method which raises the StatusChanged with e as null
        /// </summary>
        public void SetStatusFail2()
        {
            //Raise event with e as null
            StatusChanged(this, null);
        }

        /// <summary>
        /// A mock method which raises the StatusChanged sender not of type ITradeActivityItem
        /// </summary>
        public void SetStatusFail3()
        {
            //Raise event with sender not of type ITradeActivityItem
            StatusChanged(new object(), new StatusEventArgs(this.status, this.status, string.Empty, DateTime.Today));
        }
    }
}
