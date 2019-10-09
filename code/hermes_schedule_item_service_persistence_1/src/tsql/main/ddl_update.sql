ALTER TABLE activity_hist MODIFY default_start_time DATE;

--Modification for actvity table See http://forums.topcoder.com/?module=Thread&threadID=594904&start=0
ALTER TABLE activity MODIFY default_start_time NULL;
ALTER TABLE activity MODIFY last_modified_dt NULL;
ALTER TABLE activity MODIFY default_expire_days NULL;
ALTER TABLE activity MODIFY last_modified_by NULL;
ALTER TABLE activity MODIFY work_day_amt NULL;
ALTER TABLE activity MODIFY duration NULL;
ALTER TABLE activity MODIFY enabled_ind NULL;

--Modification for actvity_type table See http://forums.topcoder.com/?module=Thread&threadID=594904&start=0
ALTER TABLE activity_type MODIFY ACTIVITY_TYPE_NM NULL;
ALTER TABLE activity_type_hist MODIFY ACTIVITY_TYPE_NM NULL;


--Modification for sched_item table See http://forums.topcoder.com/?module=Thread&threadID=594904&start=0
ALTER TABLE sched_item MODIFY NOTE_ID NULL;
ALTER TABLE sched_item MODIFY SCHED_ROLE_NOTE_ID NULL;
ALTER TABLE sched_item_hist MODIFY NOTE_ID NULL;
ALTER TABLE sched_item_hist MODIFY SCHED_ROLE_NOTE_ID NULL;

commit;
