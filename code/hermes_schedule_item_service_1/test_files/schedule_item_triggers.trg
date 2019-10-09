/**********************************************************************
/*
/* Filename: schedule_item_triggers.trg
/* Component: Hermes Schedule Item Persistence
/* Designer: argolite
/* Developer: TCSDeveloper
/* Version: 1.0
/* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
/*
/* Description: The triggers for this component.
/*
/**********************************************************************/


/*********************************************************************************************************
/**
/** Trigger: dbt1_insert_ctvty_typ
/** Description: INSERT trigger for the view_activity_type view
/**              Inserts into activity_type and also inserts into the activity_type_hist table.
/*********************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt1_insert_ctvty_typ
   INSTEAD OF INSERT ON view_activity_type
   FOR EACH ROW
BEGIN
   --Insert in activity_type table.
   INSERT INTO activity_type
   (
      activity_type_id,
      activity_group_id,
      last_modified_dt,
      activity_type_nm,
      last_modified_by,
      activity_type_abbr
   )
   VALUES
   (
      :new.activity_type_id,
      :new.activity_group_id,
      :new.act_typ_last_modified_dt,
      :new.activity_type_nm,
      :new.act_typ_last_modified_by,
      :new.activity_type_abbr
   );

   --Insert in history table
   INSERT INTO activity_type_hist
   (
       activity_type_hist_id,
       activity_type_id,
       activity_group_id,
       activity_type_nm,
       last_modified_dt,
       last_modified_by,
       change_dt,
       activity_type_abbr,
       change_by, 
       action
   )
   VALUES
   (
      sys_guid(),
      :new.activity_type_id,
      :new.activity_group_id,
      :new.activity_type_nm,
      :new.act_typ_last_modified_dt,
      :new.act_typ_last_modified_by,
      (SELECT sysdate FROM dual),
      :new.activity_type_abbr,
      :new.act_typ_last_modified_by,
      'I'
   ); 
END;
/


/*********************************************************************************************************
/**
/** Trigger: dbt2_update_ctvty_typ
/** Description: Update trigger for the view_activity_type view
/**              Updates the activity_type table and inserts into the activity_type_hist table.
/*********************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt2_update_ctvty_typ
   INSTEAD OF UPDATE ON view_activity_type
   FOR EACH ROW
BEGIN
   --Update the activity_type table.
   UPDATE activity_type
   SET
      last_modified_dt = :new.act_typ_last_modified_dt,
      last_modified_by = :new.act_typ_last_modified_by,
      activity_group_ID = :new.activity_group_id,
      activity_type_abbr = :new.activity_type_abbr,
      activity_type_nm = :new.activity_type_nm
   WHERE
      activity_type_ID = :new.activity_type_id;

   --Insert in history table
   INSERT INTO activity_type_hist
   (
       activity_type_hist_id,
       activity_type_id,
       activity_group_id,
       activity_type_nm,
       last_modified_dt,
       last_modified_by,
       change_dt,
       activity_type_abbr,
       change_by, 
       action
   )
   VALUES
   (
      sys_guid(),
      :new.activity_type_id,
      :new.activity_group_id,
      :new.activity_type_nm,
      :new.act_typ_last_modified_dt,
      :new.act_typ_last_modified_by,
      (SELECT sysdate FROM dual),
      :new.activity_type_abbr,
      :new.act_typ_last_modified_by,
      'U'
   );
END;
/


/*********************************************************************************************************
/**
/** Trigger: dbt3_delete_ctvty_typ
/** Description: Delete trigger for the view_activity_type view.
/**              Deletes from the activity_type table and inserts into the activity_type_hist table.
/*********************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt3_delete_ctvty_typ
   INSTEAD OF DELETE ON view_activity_type
   FOR EACH ROW
BEGIN
   --Delete from activity type table
   DELETE FROM activity_type
   WHERE
      activity_type_Id = :old.activity_type_id;


   --Insert in history table
   INSERT INTO activity_type_hist
   (
       activity_type_hist_id,
       activity_type_id,
       activity_group_id,
       activity_type_nm,
       last_modified_dt,
       last_modified_by,
       change_dt,
       activity_type_abbr,
       change_by, 
       action
   )
   VALUES
   (
      sys_guid(),
      :old.activity_type_id,
      :old.activity_group_id,
      :old.activity_type_nm,
      (SELECT sysdate FROM dual),
      (SELECT user FROM dual),
      (SELECT sysdate FROM dual),
      :old.activity_type_abbr,
      (SELECT user FROM dual),
      'D'
   );
END;
/



/*********************************************************************************************************
/**
/** Trigger: dbt1_insert_schd_stts
/** Description: INSERT trigger for the view_schedule_item_status view
/**              Inserts into the sched_status table and inserts into the sched_status_hist table.
/*********************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt1_insert_schd_stts
   INSTEAD OF INSERT ON view_schedule_item_status
   FOR EACH ROW
BEGIN
   --Insert in sched_status table.
   INSERT INTO sched_status
   (
      sched_status_id,
      last_modified_dt,
      last_modified_by,
      status_abbr,
      status_desc   
   )
   VALUES
   (
      :new.sched_status_id,
      :new.s_last_modified_dt,
      :new.s_last_modified_by,
      :new.s_status_abbr,
      :new.s_status_desc
   );

   --Insert in history table
   INSERT INTO sched_status_hist
   (
      sched_status_hist_id,
      sched_status_id,
      last_modified_dt,
      last_modified_by,
      change_dt,
      status_abbr,
      change_by,
      status_desc,
      action
   )
   VALUES
   (
      sys_guid(),
      :new.sched_status_id,
      :new.s_last_modified_dt,
      :new.s_last_modified_by,
      (SELECT sysdate from dual),
      :new.s_status_abbr,
      :new.s_last_modified_by,
      :new.s_status_desc,
      'I'
   );
END;
/


/*********************************************************************************************************
/**
/** Trigger: dbt2_update_schd_stts
/** Description: Update trigger for the view_schedule_item_status view
/**              Updates the sched_status table and inserts into the sched_status_hist table.
/*********************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt2_update_schd_stts
   INSTEAD OF UPDATE ON view_schedule_item_status
   FOR EACH ROW
BEGIN
   --Update the sched_status table.
   UPDATE sched_status
   SET
      last_modified_dt = :new.s_last_modified_dt,
      last_modified_by = :new.s_last_modified_by,
      status_abbr = :new.s_status_abbr,
      status_desc = :new.s_status_desc
   WHERE
      sched_status_id = :new.sched_status_id;

   --Insert in history table
   INSERT INTO sched_status_hist
   (
      sched_status_hist_id,
      sched_status_id,
      last_modified_dt,
      last_modified_by,
      change_dt,
      status_abbr,
      change_by,
      status_desc,
      action
   )
   VALUES
   (
      sys_guid(),
      :new.sched_status_id,
      :new.s_last_modified_dt,
      :new.s_last_modified_by,
      (SELECT sysdate from dual),
      :new.s_status_abbr,
      :new.s_last_modified_by,
      :new.s_status_desc,
      'U'
   );
END;
/


/*********************************************************************************************************
/**
/** Trigger: dbt3_delete_schd_stts
/** Description: Delete trigger for the view_schedule_item_status view
/**              Deletes from the sched_status table and inserts into the sched_status_hist table.
/*********************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt3_delete_schd_stts
   INSTEAD OF DELETE ON view_schedule_item_status
   FOR EACH ROW
BEGIN
   --Delete from sched_status table
   DELETE FROM sched_status
   WHERE
      sched_status_id = :old.sched_status_id;

   --Insert in history table
   INSERT INTO sched_status_hist
   (
      sched_status_hist_id,
      sched_status_id,
      last_modified_dt,
      last_modified_by,
      change_dt,
      status_abbr,
      change_by,
      status_desc,
      action
   )
   VALUES
   (
      sys_guid(),
      :old.sched_status_id,
      (SELECT sysdate from dual),
      (SELECT user FROM dual),
      (SELECT sysdate from dual),
      :old.s_status_abbr,
      (SELECT user FROM dual),
      :old.s_status_desc,
      'D'
   );
END;
/


/*********************************************************************************************************
/**
/** Trigger: dbt1_insert_ctvty_grp
/** Description: INSERT trigger for the view_activity_group view
/**              Inserts into the activity_group table and inserts into the activity_group_hist table.
/*********************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt1_insert_ctvty_grp
   INSTEAD OF INSERT ON view_activity_group
   FOR EACH ROW
BEGIN
   --Insert in activity_group table.
   INSERT INTO activity_group
   (
      activity_group_id,
      last_modified_dt,
      last_modified_by,
      act_grp_abbr,
      act_grp_nm   
   )
   VALUES
   (
      :new.activity_group_id,
      :new.act_grp_last_modified_dt,
      :new.act_grp_last_modified_by,
      :new.act_grp_abbr,
      :new.act_grp_nm
   );

   --Insert in history table
   INSERT INTO activity_group_hist
   (
      activity_group_hist_id,
      activity_group_id,
      last_modified_dt,
      last_modified_by, 
      act_grp_nm,
      change_dt,
      change_by,
      action,
      act_grp_abbr
   )
   VALUES
   (
      sys_guid(),
      :new.activity_group_ID,
      :new.act_grp_last_modified_dt,
      :new.act_grp_last_modified_by,
      :new.act_grp_nm,
      (SELECT sysdate FROM dual),
      :new.act_grp_last_modified_by,
      'I',
      :new.act_grp_abbr
   );
END;
/


/*********************************************************************************************************
/**
/** Trigger: dbt2_update_ctvty_grp
/** Description: Update trigger for the view_activity_group view
/**              Updates the activity_group table and inserts into the activity_group_hist table.
/*********************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt2_update_ctvty_grp
   INSTEAD OF UPDATE ON view_activity_group
   FOR EACH ROW
BEGIN
   --Update the activity_group table.
   UPDATE activity_group
   SET
      last_modified_dt = :new.act_grp_last_modified_dt,
      last_modified_by = :new.act_grp_last_modified_by,
      act_grp_abbr = :new.act_grp_abbr,
      act_grp_nm = :new.act_grp_nm
   WHERE
      activity_group_id = :new.activity_group_id;

   --Insert in history table
   INSERT INTO activity_group_hist
   (
      activity_group_hist_id,
      activity_group_id,
      last_modified_dt,
      last_modified_by, 
      act_grp_nm,
      change_dt,
      change_by,
      action,
      act_grp_abbr
   )
   VALUES
   (
      sys_guid(),
      :new.activity_group_ID,
      :new.act_grp_last_modified_dt,
      :new.act_grp_last_modified_by,
      :new.act_grp_nm,
      (SELECT sysdate FROM dual),
      :new.act_grp_last_modified_by,
      'U',
      :new.act_grp_abbr
   );
END;
/


/*********************************************************************************************************
/**
/** Trigger: dbt3_delete_ctvty_grp
/** Description: Delete trigger for the view_activity_group view
/**              Deletes from the activity_group table and inserts into the activity_group_hist table.
/*********************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt3_delete_ctvty_grp
   INSTEAD OF DELETE ON view_activity_group
   FOR EACH ROW
BEGIN
   --Delete from activity_group table
   DELETE FROM activity_group
   WHERE
      activity_group_id = :old.activity_group_id;

   --Insert in history table
   INSERT INTO activity_group_hist
   (
      activity_group_hist_id,
      activity_group_id,
      last_modified_dt,
      last_modified_by, 
      act_grp_nm,
      change_dt,
      change_by,
      action,
      act_grp_abbr
   )
   VALUES
   (
      sys_guid(),
      :old.activity_group_ID,
      (SELECT sysdate from dual),
      (SELECT user FROM dual),
      :old.act_grp_nm,
      (SELECT sysdate FROM dual),
      (SELECT user FROM dual),
      'D',
      :old.act_grp_abbr
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt1_insert_schd_req_stts
/** Description: INSERT trigger for the view_sched_item_req_status view
/**              Inserts into the sched_request_status table and inserts into the sched_request_status_hist table.
/*********************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt1_insert_schd_req_stts
   INSTEAD OF INSERT ON view_sched_item_req_status
   FOR EACH ROW
BEGIN
   --Insert in sched_request_status table.
   INSERT INTO sched_request_status
   (
      sched_request_status_id,
      last_modified_dt,
      last_modified_by,
      status_desc,
      status_abbr   
   )
   VALUES
   (
      :new.sched_request_status_id,
      :new.rs_last_modified_dt,
      :new.rs_last_modified_by,
      :new.rs_status_desc,
      :new.rs_status_abbr
   );

   --Insert in history table
   INSERT INTO sched_request_status_hist
   (
      sched_request_status_hist_id,
      sched_request_status_id,
      last_modified_dt, 
      last_modified_by,
      change_dt,
      status_abbr,
      change_by,
      status_desc,
      action
   )
   VALUES
   (
      sys_guid(),
      :new.sched_request_status_id,
      :new.rs_last_modified_dt,
      :new.rs_last_modified_by,
      (SELECT sysdate from dual),
      :new.rs_status_abbr,
      :new.rs_last_modified_by,
      :new.rs_status_desc,
      'I'
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt2_update_schd_req_stts
/** Description: Update trigger for the view_sched_item_req_status view
/**              Updates the sched_request_status table and inserts into the sched_request_status_hist table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt2_update_schd_req_stts
   INSTEAD OF UPDATE ON view_sched_item_req_status
   FOR EACH ROW
BEGIN
   --Update the sched_request_status table.
   UPDATE sched_request_status
   SET
      last_modified_dt = :new.rs_last_modified_dt,
      last_modified_by = :new.rs_last_modified_by,
      status_abbr = :new.rs_status_abbr,
      status_desc = :new.rs_status_desc
   WHERE
      sched_request_status_id = :new.sched_request_status_id;

   --Insert in history table
   INSERT INTO sched_request_status_hist
   (
      sched_request_status_hist_id,
      sched_request_status_id,
      last_modified_dt, 
      last_modified_by,
      change_dt,
      status_abbr,
      change_by,
      status_desc,
      action
   )
   VALUES
   (
      sys_guid(),
      :new.sched_request_status_id,
      :new.rs_last_modified_dt,
      :new.rs_last_modified_by,
      (SELECT sysdate from dual),
      :new.rs_status_abbr,
      :new.rs_last_modified_by,
      :new.rs_status_desc,
      'U'
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt3_delete_schd_req_stts
/** Description: Delete trigger for the view_sched_item_req_status view
/**              Deletes from the sched_request_status table and inserts into the sched_request_status_hist table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt3_delete_schd_req_stts
   INSTEAD OF DELETE ON view_sched_item_req_status
   FOR EACH ROW
BEGIN
   --Delete from sched_request_status table
   DELETE FROM sched_request_status
   WHERE
      sched_request_status_id = :old.sched_request_status_id;

   --Insert in history table
   INSERT INTO sched_request_status_hist
   (
      sched_request_status_hist_id,
      sched_request_status_id,
      last_modified_dt, 
      last_modified_by,
      change_dt,
      status_abbr,
      change_by,
      status_desc,
      action
   )
   VALUES
   (
      sys_guid(),
      :old.sched_request_status_id,
      (SELECT sysdate from dual),
      (SELECT user FROM dual),
      (SELECT sysdate from dual),
      :old.rs_status_abbr,
      (SELECT user FROM dual),
      :old.rs_status_desc,
      'D'
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt1_insert_ctvty
/** Description: INSERT trigger for the view_activity view
/**              Inserts into the activity table and inserts into the activity_hist table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt1_insert_ctvty
   INSTEAD OF INSERT ON view_activity
   FOR EACH ROW
BEGIN
   --Insert in activity table.
   INSERT INTO activity
   (
      activity_id,
      activity_type_id,
      default_start_time,
      last_modified_dt,
      default_expire_days,
      last_modified_by,
      work_day_amt,
      duration,
      activity_nm,
      enabled_ind,
      activity_abbr,
      exclusive_ind   
   )
   VALUES
   (
      :new.activity_id,
      :new.activity_type_id,
      :new.default_start_time,
      :new.act_last_modified_dt,
      :new.default_expire_days,
      :new.act_last_modified_by,
      :new.act_work_day_amt,
      :new.act_duration,
      :new.activity_nm,
      :new.enabled_ind,
      :new.activity_abbr,
      :new.exclusive_ind
   );

   --Insert in history table
   INSERT INTO activity_hist
   (
      activity_hist_id,
      activity_id,
      default_start_time,
      default_expire_days, 
      last_modified_dt,
      work_day_amt,
      duration,
      last_modified_by,
      activity_nm, 
      enabled,
      activity_type_id,
      activity_abbr,
      change_by,
      action,
      change_dt, 
      exclusive_ind
   )
   VALUES
   (
      sys_guid(),
      :new.activity_id,
      :new.default_start_time,
      :new.default_expire_days,
      :new.act_last_modified_dt,
      :new.act_work_day_amt,
      :new.act_duration,
      :new.act_last_modified_by,
      :new.activity_nm,
      :new.enabled_ind,
      :new.activity_type_id,
      :new.activity_abbr,
      :new.act_last_modified_by,
      'I',
      (SELECT sysdate from dual),
      :new.exclusive_ind
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt2_update_ctvty
/** Description: Update trigger for the view_activity view
/**              Updates the activity table and inserts into the activity_hist table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt2_update_ctvty
   INSTEAD OF UPDATE ON view_activity
   FOR EACH ROW
BEGIN
   --Update the activity table.
   UPDATE activity
   SET
      activity_type_id = :new.activity_type_id,
      default_start_time = :new.default_start_time,
      last_modified_dt = :new.act_last_modified_dt,
      default_expire_days = :new.default_expire_days,
      last_modified_by = :new.act_last_modified_by,
      work_day_amt = :new.act_work_day_amt,
      duration = :new.act_duration,
      activity_nm = :new.activity_nm,
      enabled_ind = :new.enabled_ind,
      activity_abbr = :new.activity_abbr,
      exclusive_ind = :new.exclusive_ind
   WHERE
      activity_id = :new.activity_id;

   --Insert in history table
   INSERT INTO activity_hist
   (
      activity_hist_id,
      activity_id,
      default_start_time,
      default_expire_days, 
      last_modified_dt,
      work_day_amt,
      duration,
      last_modified_by,
      activity_nm, 
      enabled,
      activity_type_id,
      activity_abbr,
      change_by,
      action,
      change_dt, 
      exclusive_ind
   )
   VALUES
   (
      sys_guid(),
      :new.activity_id,
      :new.default_start_time,
      :new.default_expire_days,
      :new.act_last_modified_dt,
      :new.act_work_day_amt,
      :new.act_duration,
      :new.act_last_modified_by,
      :new.activity_nm,
      :new.enabled_ind,
      :new.activity_type_id,
      :new.activity_abbr,
      :new.act_last_modified_by,
      'U',
      (SELECT sysdate from dual),
      :new.exclusive_ind
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt3_delete_ctvty
/** Description: Delete trigger for the view_activity view
/**              Deletes from the activity table and inserts into the activity table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt3_delete_ctvty
   INSTEAD OF DELETE ON view_activity
   FOR EACH ROW
BEGIN
   --Delete from activity table
   DELETE FROM activity
   WHERE
      activity_id = :old.activity_id;

   --Insert in history table
   INSERT INTO activity_hist
   (
      activity_hist_id,
      activity_id,
      default_start_time,
      default_expire_days, 
      last_modified_dt,
      work_day_amt,
      duration,
      last_modified_by,
      activity_nm, 
      enabled,
      activity_type_id,
      activity_abbr,
      change_by,
      action,
      change_dt, 
      exclusive_ind
   )
   VALUES
   (
      sys_guid(),
      :old.activity_id,
      :old.default_start_time,
      :old.default_expire_days,
      (SELECT sysdate from dual),
      :old.act_work_day_amt,
      :old.act_duration,
      (SELECT user from dual),
      :old.activity_nm,
      :old.enabled_ind,
      :old.activity_type_id,
      :old.activity_abbr,
      (SELECT user from dual),
      'D',
      (SELECT sysdate from dual),
      :old.exclusive_ind
   );
END;
/



/*****************************************************************************************************************
/**
/** Trigger: dbt1_insert_schd_tm
/** Description: INSERT trigger for the view_schedule_item view
/**              Inserts into the sched_item table and inserts into the sched_item_hist table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt1_insert_schd_tm
   INSTEAD OF INSERT ON view_schedule_item
   FOR EACH ROW
BEGIN
   --Insert in sched_item table.
   INSERT INTO sched_item
   (
      schedule_item_id,
      sched_request_status_id,
      sched_status_id,
      note_id, 
      sched_role_note_id,
      activity_id,
      last_modified_dt,
      work_day_amt,
      last_modified_by, 
      version,
      duration,
      exception_ind,
      expire_dt,
      work_dt
   )
   VALUES
   (
      :new.schedule_item_id,
      :new.sched_request_status_id,
      :new.sched_status_id,
      :new.note_id,
      :new.sched_role_note_id,
      :new.activity_id,
      :new.last_modified_dt,
      :new.work_day_amt,
      :new.last_modified_by,
      :new.version,
      :new.duration,
      :new.exception_ind,
      :new.expire_dt,
      :new.work_dt
   );

   --Insert in history table
   INSERT INTO sched_item_hist
   (
      sched_item_hist_id,
      schedule_item_id,
      activity_id,
      note_id,
      sched_role_note_id, 
      sched_request_status_id,
      sched_status_id,
      work_day_amount,
      version, 
      last_modified_date,
      duration,
      last_modified_by,
      change_dt,
      expire_dt, 
      change_by,
      action,
      exception_flag,
      work_dt
   )
   VALUES
   (
      sys_guid(),
      :new.schedule_item_id,
      :new.activity_id,
      :new.note_id,
      :new.sched_role_note_id,
      :new.sched_request_status_id,
      :new.sched_status_id,
      :new.work_day_amt,
      :new.version,
      :new.last_modified_dt,
      :new.duration,
      :new.last_modified_by,
      (SELECT sysdate FROM dual),
      :new.expire_dt,
      :new.last_modified_by,
      'I',
      :new.exception_ind,
      :new.work_dt
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt2_update_schd_tm
/** Description: Update trigger for the view_schedule_item view
/**              Updates the sched_item table and inserts into the sched_item_hist table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt2_update_schd_tm
   INSTEAD OF UPDATE ON view_schedule_item
   FOR EACH ROW
BEGIN
   --Update the sched_item table.
   UPDATE sched_item
   SET
      sched_request_status_id = :new.sched_request_status_id,
      sched_status_id = :new.sched_status_id,
      note_id = :new.note_id,
      sched_role_note_id = :new.sched_role_note_id,
      activity_id = :new.activity_id,
      last_modified_dt = :new.last_modified_dt,
      work_day_amt = :new.work_day_amt,
      last_modified_by = :new.last_modified_by,
      version = :new.version,
      duration = :new.duration,
      exception_ind = :new.exception_ind,
      expire_dt = :new.expire_dt,
      work_dt = :new.work_dt
   WHERE
      schedule_item_id = :new.schedule_item_id;

   --Insert in history table
   INSERT INTO sched_item_hist
   (
      sched_item_hist_id,
      schedule_item_id,
      activity_id,
      note_id,
      sched_role_note_id, 
      sched_request_status_id,
      sched_status_id,
      work_day_amount,
      version, 
      last_modified_date,
      duration,
      last_modified_by,
      change_dt,
      expire_dt, 
      change_by,
      action,
      exception_flag,
      work_dt
   )
   VALUES
   (
      sys_guid(),
      :new.schedule_item_id,
      :new.activity_id,
      :new.note_id,
      :new.sched_role_note_id,
      :new.sched_request_status_id,
      :new.sched_status_id,
      :new.work_day_amt,
      :new.version,
      :new.last_modified_dt,
      :new.duration,
      :new.last_modified_by,
      (SELECT sysdate FROM dual),
      :new.expire_dt,
      :new.last_modified_by,
      'U',
      :new.exception_ind,
      :new.work_dt
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt3_delete_schd_tm
/** Description: Delete trigger for the view_schedule_item view
/**              Deletes from the sched_item table and inserts into the sched_item_hist table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt3_delete_schd_tm
   INSTEAD OF DELETE ON view_schedule_item
   FOR EACH ROW
BEGIN
   --Delete from sched_item table
   DELETE FROM sched_item
   WHERE
      schedule_item_id = :old.schedule_item_id;

   --Insert in history table
   INSERT INTO sched_item_hist
   (
      sched_item_hist_id,
      schedule_item_id,
      activity_id,
      note_id,
      sched_role_note_id, 
      sched_request_status_id,
      sched_status_id,
      work_day_amount,
      version, 
      last_modified_date,
      duration,
      last_modified_by,
      change_dt,
      expire_dt, 
      change_by,
      action,
      exception_flag,
      work_dt
   )
   VALUES
   (
      sys_guid(),
      :old.schedule_item_id,
      :old.activity_id,
      :old.note_id,
      :old.sched_role_note_id,
      :old.sched_request_status_id,
      :old.sched_status_id,
      :old.work_day_amt,
      :old.version,
      (SELECT sysdate FROM dual),
      :old.duration,
      (SELECT user FROM dual),
      (SELECT sysdate FROM dual),
      :old.expire_dt,
      (SELECT user FROM dual),
      'D',
      :old.exception_ind,
      :old.work_dt
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt1_insert_schd_itm_pub_xrf
/** Description: INSERT trigger for the view_publish_xref view
/**              Inserts in sched_item_publish_xref table and inserts into the sched_item_publish_xref_hist table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt1_insert_schd_itm_pub_xrf
   INSTEAD OF INSERT ON view_publish_xref
   FOR EACH ROW
DECLARE
   new_xref_id  RAW(16);
BEGIN
   SELECT sys_guid() INTO new_xref_id FROM dual;

   --Insert in sched_item_publish_xref table.
   INSERT INTO sched_item_publish_xref
   (
      sched_item_publish_xref_id,
      published_schedule_item_id,
      edit_schedule_item_id, 
      last_modified_dt,
      last_modified_by
   )
   VALUES
   (
      new_xref_id,
      :new.published_schedule_item_id,
      :new.edit_schedule_item_id,
      :new.last_modified_dt,
      :new.last_modified_by
   );

   --Insert in history
   INSERT INTO sched_item_pub_xref_hist
   (
      sched_item_pub_xref_hist_id,
      sched_item_pub_xref_id,
      published_schedule_item_id, 
      edit_schedule_item_id,
      last_modified_dt,
      last_modified_by,
      change_dt, 
      change_by,
      action
   )
   VALUES
   (
      sys_guid(),
      new_xref_id,
      :new.published_schedule_item_id,
      :new.edit_schedule_item_id,
      :new.last_modified_dt,
      :new.last_modified_by,
      (SELECT sysdate from dual),
      :new.last_modified_by,
      'I'
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt2_delete_schd_itm_pub_xrf
/** Description: Delete trigger for the view_publish_xref view
/**              Deletes from sched_item_publish_xref table and inserts into the sched_item_publish_xref_hist table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt2_delete_schd_itm_pub_xrf
   INSTEAD OF DELETE ON view_publish_xref
   FOR EACH ROW
BEGIN
   --Delete from sched_item_publish_xref table
   DELETE FROM sched_item_publish_xref
   WHERE
      edit_schedule_item_id = :old.edit_schedule_item_id;

   --Insert in history
   INSERT INTO sched_item_pub_xref_hist
   (
      sched_item_pub_xref_hist_id,
      sched_item_pub_xref_id,
      published_schedule_item_id, 
      edit_schedule_item_id,
      last_modified_dt,
      last_modified_by,
      change_dt, 
      change_by,
      action
   )
   VALUES
   (
      sys_guid(),
      :old.sched_item_publish_xref_id,
      :old.published_schedule_item_id,
      :old.edit_schedule_item_id,
      (SELECT sysdate from dual),
      (SELECT user from dual),
      (SELECT sysdate from dual),
      (SELECT user from dual),
      'D'
   );
END;
/


/*****************************************************************************************************************
/**
/** Trigger: dbt3_update_schd_itm_pub_xrf
/** Description: Update trigger for the view_publish_xref view
/**              Updates the sched_item_publish_xref table and inserts into the sched_item_publish_xref_hist table.
/*****************************************************************************************************************/
CREATE OR REPLACE TRIGGER dbt3_update_schd_itm_pub_xrf
   INSTEAD OF UPDATE ON view_publish_xref
   FOR EACH ROW
BEGIN
   --Update in sched_item_publish_xref table.
   UPDATE sched_item_publish_xref
   SET
      edit_schedule_item_ID = :new.edit_schedule_item_ID,
      published_schedule_item_ID = :new.published_schedule_item_ID,
      last_modified_dt = :new.last_modified_dt,
      last_modified_by = :new.last_modified_by
   WHERE
      sched_item_publish_xref_ID = :new.sched_item_publish_xref_ID;
   
   --Insert in history
   INSERT INTO sched_item_pub_xref_hist
   (
      sched_item_pub_xref_hist_id,
      sched_item_pub_xref_id,
      published_schedule_item_id, 
      edit_schedule_item_id,
      last_modified_dt,
      last_modified_by,
      change_dt, 
      change_by,
      action
   )
   VALUES
   (
      sys_guid(),
      :new.sched_item_publish_xref_ID,
      :new.published_schedule_item_ID,
      :new.edit_schedule_item_ID,
      :new.last_modified_dt,
      :new.last_modified_by,
      (SELECT sysdate from dual),
      :new.last_modified_by,
      'U'
   );
END;
/
