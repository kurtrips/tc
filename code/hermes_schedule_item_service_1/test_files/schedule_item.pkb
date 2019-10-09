/************************************************************************************************************
/*
/* Filename: schedule_item.pkb
/* Component: Hermes Schedule Item Persistence
/* Package:  schedule_item
/* Designer: argolite
/* Developer: TCSDeveloper
/* Version: 1.0
/* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
/*
/* Description: The package body of the schedule_item package. Contains the actual
/*             implementation of the procedures of the schedule_item package.
/*
/************************************************************************************************************/
CREATE OR REPLACE PACKAGE BODY schedule_item AS

   /*********************************************************************************************************
   /**
   /** Procedure: sp_save_activity_group
   /** Description: Creates or updates an activity group record depending on
   /**              whether record with id as p_activity_group_ID exists or not.
   /** In: p_activity_group_ID - The ID of the activity group.
   /** In: p_last_modified_dt - The last modified date.
   /** In: p_last_modified_by - The user by whom it was last modified.
   /** In: p_act_grp_abbr - The abbreviation of the activity group.
   /** In: p_act_grp_nm - The name of the activity group.
   /*********************************************************************************************************/
   PROCEDURE sp_save_activity_group
   (
      p_activity_group_ID  view_activity_group.ACTIVITY_GROUP_ID%TYPE,
      p_last_modified_dt   view_activity_group.ACT_GRP_LAST_MODIFIED_DT%TYPE,
      p_last_modified_by   view_activity_group.ACT_GRP_LAST_MODIFIED_BY%TYPE,
      p_act_grp_abbr       view_activity_group.ACT_GRP_ABBR%TYPE,
      p_act_grp_nm         view_activity_group.ACT_GRP_NM%TYPE
   )
   IS
      numRecs         NUMBER;
   BEGIN
      SELECT COUNT(1) INTO numRecs
      FROM view_activity_group
      WHERE
         activity_group_ID = p_activity_group_ID;
      
      IF numRecs > 0
      THEN
         UPDATE view_activity_group
         SET
            activity_group_ID = p_activity_group_ID,
            act_grp_last_modified_dt = p_last_modified_dt,
            act_grp_last_modified_by = p_last_modified_by,
            act_grp_abbr = p_act_grp_abbr,
            act_grp_nm = p_act_grp_nm
         WHERE
            activity_group_ID = p_activity_group_ID;
      ELSE
         INSERT INTO view_activity_group
         VALUES
         (
            p_activity_group_ID,
            p_last_modified_dt,
            p_last_modified_by,
            p_act_grp_abbr,
            p_act_grp_nm
         );
      END IF;
   END sp_save_activity_group;
   
   
   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_activity_group
   /** Description: Gets the data of an activity group record with id as p_activity_group_ID.
   /** In: p_activity_group_ID - The ID of the activity group.
   /** Out: p_results_cursor - Cursor containing the row data for the given id.
   /*********************************************************************************************************/
   PROCEDURE sp_get_activity_group
   (
      p_activity_group_ID  view_activity_group.ACTIVITY_GROUP_ID%TYPE,
      p_results_cursor     OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_activity_group
         WHERE
            activity_group_ID = p_activity_group_ID;
   END sp_get_activity_group;


   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_all_activity_groups
   /** Description: Gets the data of all activity group records in the database.
   /** Out: p_results_cursor - Cursor containing the data for all activity groups.
   /*********************************************************************************************************/
   PROCEDURE sp_get_all_activity_groups
   (
      p_results_cursor        OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_activity_group;
   END sp_get_all_activity_groups;


   /*********************************************************************************************************
   /**
   /** Procedure: sp_delete_activity_group
   /** Description: Deletes an activity group record with id as p_activity_group_ID.
   /** In: p_activity_group_ID - The ID of the activity group.
   /** InOut: p_rows_affected - The number of rows deleted.
   /*********************************************************************************************************/
   PROCEDURE sp_delete_activity_group
   (
      p_activity_group_ID   view_activity_group.ACTIVITY_GROUP_ID%TYPE,
      p_rows_affected       IN OUT NUMBER
   )
   IS
   BEGIN
      SELECT COUNT(1) INTO p_rows_affected
      FROM view_activity_group
      WHERE activity_group_ID = p_activity_group_ID;

      DELETE FROM view_activity_group
      WHERE
         activity_group_ID = p_activity_group_ID;
   END sp_delete_activity_group;


   /*********************************************************************************************************
   /**
   /** Procedure: sp_save_activity_type
   /** Description: Creates or updates an activity type record depending on
   /**              whether record with id as p_activity_type_ID exists or not.
   /** In: p_activity_type_ID - The ID of the activity type.
   /** In: p_activity_group_ID - The ID of the activity group for the activity type.
   /** In: p_activity_type_abbr - The abbreviation of the activity type.   
   /** In: p_last_modified_date - The last modified date.
   /** In: p_last_modified_by - The user by whom it was last modified.
   /** In: p_activity_type_nm - The name of the activity type.
   /*********************************************************************************************************/
   PROCEDURE sp_save_activity_type
   (
      p_activity_type_ID    view_activity_type.ACTIVITY_TYPE_ID%TYPE,
      p_activity_group_ID   view_activity_type.ACTIVITY_GROUP_ID%TYPE,
      p_activity_type_abbr  view_activity_type.ACTIVITY_TYPE_ABBR%TYPE,
      p_last_modified_date  view_activity_type.ACT_TYP_LAST_MODIFIED_DT%TYPE,
      p_last_modified_by    view_activity_type.ACT_TYP_LAST_MODIFIED_BY%TYPE,
      p_activity_type_nm    view_activity_type.ACTIVITY_TYPE_NM%TYPE
   )
   IS
      numRecs   NUMBER;
   BEGIN
      SELECT COUNT(*) INTO numRecs
      FROM view_activity_type
      WHERE
         activity_type_ID = p_activity_type_ID;
      
      IF numRecs > 0
      THEN
         UPDATE view_activity_type
         SET
            act_typ_last_modified_dt = p_last_modified_date,
            act_typ_last_modified_by = p_last_modified_by,
            activity_group_ID = p_activity_group_ID,
            activity_type_abbr = p_activity_type_abbr,
            activity_type_nm = p_activity_type_nm
         WHERE
            activity_type_ID = p_activity_type_ID;
      ELSE
         INSERT INTO view_activity_type 
         (
            activity_type_ID,
            act_typ_last_modified_dt,
            act_typ_last_modified_by,
            activity_type_abbr,
            activity_type_nm,
            activity_group_ID
         )
         VALUES
         (
            p_activity_type_ID,
            p_last_modified_date,
            p_last_modified_by,
            p_activity_type_abbr,
            p_activity_type_nm,
            p_activity_group_ID
         );
      END IF;
   END sp_save_activity_type;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_delete_activity_type
   /** Description: Deletes an activity type record with id as p_activity_type_ID.
   /** In: p_activity_type_ID - The ID of the activity type to delete.
   /** InOut: p_rows_affected - The number of rows deleted.
   /*********************************************************************************************************/
   PROCEDURE sp_delete_activity_type
   (
      p_activity_type_ID    view_activity_type.ACTIVITY_TYPE_ID%TYPE,
      p_rows_affected       IN OUT NUMBER
   )
   IS
   BEGIN
      SELECT COUNT(1) INTO p_rows_affected
      FROM view_activity_type
      WHERE activity_type_ID = p_activity_type_ID;

      DELETE FROM view_activity_type
      WHERE activity_type_ID = p_activity_type_ID;
   END sp_delete_activity_type;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_activity_type
   /** Description: Gets the data of an activity type record with id as p_activity_type_ID.
   /** In: p_activity_type_ID - The ID of the activity type.
   /** Out: p_results_cursor - Cursor containing the row data for the given id.
   /*********************************************************************************************************/
   PROCEDURE sp_get_activity_type
   (
      p_activity_type_ID      view_activity_type.ACTIVITY_TYPE_ID%TYPE,
      p_results_cursor        OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_activity_type
         WHERE
            activity_type_ID = p_activity_type_ID;
   END sp_get_activity_type;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_all_activity_types
   /** Description: Gets the data of all activity type records in the database.
   /** Out: p_results_cursor - Cursor containing the data for all activity types.
   /*********************************************************************************************************/
   PROCEDURE sp_get_all_activity_types
   (
      p_results_cursor        OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_activity_type;
   END sp_get_all_activity_types;


   /*********************************************************************************************************
   /**
   /** Procedure: sp_save_schedule_item_status
   /** Description: Creates or updates an schedule item status record depending on
   /**              whether record with id as p_sched_status_ID exists or not.
   /** In: p_sched_status_ID - The ID of the schedule item status.
   /** In: p_last_modified_dt - The last modified date.
   /** In: p_last_modified_by - The user by whom it was last modified.
   /** In: p_status_desc - The description of the schedule item status.
   /** In: p_status_abbr - The abbreviation of the schedule item status.
   /*********************************************************************************************************/
   PROCEDURE sp_save_schedule_item_status
   (
      p_sched_status_ID       view_schedule_item_status.SCHED_STATUS_ID%TYPE,
      p_last_modified_dt      view_schedule_item_status.S_LAST_MODIFIED_DT%TYPE,
      p_last_modified_by      view_schedule_item_status.S_LAST_MODIFIED_BY%TYPE,
      p_status_desc           view_schedule_item_status.S_STATUS_DESC%TYPE,
      p_status_abbr           view_schedule_item_status.S_STATUS_ABBR%TYPE
   )
   IS
      numRecs   NUMBER;
   BEGIN
      SELECT COUNT(*) INTO numRecs
      FROM view_schedule_item_status
      WHERE
         sched_status_ID = p_sched_status_ID;

      IF numRecs > 0
      THEN
         UPDATE view_schedule_item_status
         SET
            s_last_modified_dt = p_last_modified_dt,
            s_last_modified_by = p_last_modified_by,
            s_status_desc = p_status_desc,
            s_status_abbr = p_status_abbr
         WHERE
            sched_status_ID = p_sched_status_ID;
      ELSE
         INSERT INTO view_schedule_item_status
         (
            sched_status_ID,
            s_last_modified_by,
            s_last_modified_dt,
            s_status_abbr,
            s_status_desc
         )
         VALUES
         (
            p_sched_status_ID,
            p_last_modified_by,
            p_last_modified_dt,
            p_status_abbr,
            p_status_desc
         );
      END IF;
   END sp_save_schedule_item_status;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_delete_schedule_item_status
   /** Description: Deletes an schedule item status record with id as p_sched_status_ID.
   /** In: p_sched_status_ID - The ID of the schedule item status to delete.
   /** InOut: p_rows_affected - The number of rows deleted.
   /*********************************************************************************************************/
   PROCEDURE sp_delete_schedule_item_status
   (
      p_sched_status_ID       view_schedule_item_status.SCHED_STATUS_ID%TYPE,
      p_rows_affected         IN OUT NUMBER
   )
   IS
   BEGIN
      SELECT COUNT(1) INTO p_rows_affected FROM view_schedule_item_status
      WHERE
         sched_status_ID = p_sched_status_ID;

      DELETE FROM view_schedule_item_status
      WHERE
         sched_status_ID = p_sched_status_ID;
   END sp_delete_schedule_item_status;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_schedule_item_status
   /** Description: Gets the data of an schedule item status record with id as p_sched_status_ID.
   /** In: p_sched_status_ID - The ID of the schedule item status.
   /** Out: p_results_cursor - Cursor containing the row data for the given id.
   /*********************************************************************************************************/
   PROCEDURE sp_get_schedule_item_status
   (
      p_sched_status_ID      view_schedule_item_status.SCHED_STATUS_ID%TYPE,
      p_results_cursor       OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_schedule_item_status
         WHERE
            sched_status_ID = p_sched_status_ID;
   END sp_get_schedule_item_status;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_all_sched_item_stats
   /** Description: Gets the data of all schedule item status records in the database.
   /** Out: p_results_cursor - Cursor containing the data for all schedule item statuses.
   /*********************************************************************************************************/
   PROCEDURE sp_get_all_sched_item_stats
   (
      p_results_cursor        OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_schedule_item_status;
   END sp_get_all_sched_item_stats;

   /*********************************************************************************************************
   /**
   /** Procedure: sp_save_sched_item_req_stat
   /** Description: Creates or updates an schedule item request status record depending on
   /**              whether record with id as p_sched_req_status_ID exists or not.
   /** In: p_sched_req_status_ID - The ID of the schedule item request status.
   /** In: p_last_modified_dt - The last modified date.
   /** In: p_last_modified_by - The user by whom it was last modified.
   /** In: p_status_desc - The description of the schedule item request status.
   /** In: p_status_abbr - The abbreviation of the schedule item request status.
   /*********************************************************************************************************/
   PROCEDURE sp_save_sched_item_req_stat
   (
      p_sched_req_status_ID   view_sched_item_req_status.SCHED_REQUEST_STATUS_ID%TYPE,
      p_last_modified_dt      view_sched_item_req_status.RS_LAST_MODIFIED_DT%TYPE,
      p_last_modified_by      view_sched_item_req_status.RS_LAST_MODIFIED_BY%TYPE,
      p_status_desc           view_sched_item_req_status.RS_STATUS_DESC%TYPE,
      p_status_abbr           view_sched_item_req_status.RS_STATUS_ABBR%TYPE
   )
   IS
      numRecs   NUMBER;
   BEGIN
      SELECT COUNT(*) INTO numRecs
      FROM view_sched_item_req_status
      WHERE
         sched_request_status_ID = p_sched_req_status_ID;

      IF numRecs > 0
      THEN
         UPDATE view_sched_item_req_status
         SET
            rs_last_modified_dt = p_last_modified_dt,
            rs_last_modified_by = p_last_modified_by,
            rs_status_desc = p_status_desc,
            rs_status_abbr = p_status_abbr
         WHERE
            sched_request_status_ID = p_sched_req_status_ID;
      ELSE
         INSERT INTO view_sched_item_req_status
         (
            sched_request_status_ID,
            rs_last_modified_by,
            rs_last_modified_dt,
            rs_status_desc,
            rs_status_abbr
         )
         VALUES
         (
            p_sched_req_status_ID,
            p_last_modified_by,
            p_last_modified_dt,
            p_status_desc,
            p_status_abbr
         );
      END IF;
   END sp_save_sched_item_req_stat;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_delete_sched_item_req_stat
   /** Description: Deletes an schedule item request status record with id as p_sched_req_status_ID.
   /** In: p_sched_req_status_ID - The ID of the schedule item request status to delete.
   /** InOut: p_rows_affected - The number of rows deleted.
   /*********************************************************************************************************/
   PROCEDURE sp_delete_sched_item_req_stat
   (
      p_sched_req_status_ID   view_sched_item_req_status.SCHED_REQUEST_STATUS_ID%TYPE,
      p_rows_affected         IN OUT NUMBER
   )
   IS
   BEGIN
      SELECT COUNT(1) INTO p_rows_affected
      FROM view_sched_item_req_status
      WHERE sched_request_status_ID = p_sched_req_status_ID;

      DELETE FROM view_sched_item_req_status
      WHERE
         sched_request_status_ID = p_sched_req_status_ID;
   END sp_delete_sched_item_req_stat;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_sched_item_req_stat
   /** Description: Gets the data of an schedule item request status record with id as p_sched_req_status_ID.
   /** In: p_sched_req_status_ID - The ID of the schedule item request status.
   /** Out: p_results_cursor - Cursor containing the row data for the given id.
   /*********************************************************************************************************/
   PROCEDURE sp_get_sched_item_req_stat
   (
      p_sched_req_status_ID   view_sched_item_req_status.SCHED_REQUEST_STATUS_ID%TYPE,
      p_results_cursor        OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_sched_item_req_status
         WHERE
            sched_request_status_ID = p_sched_req_status_ID;
   END sp_get_sched_item_req_stat;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_all_sched_item_req_stat
   /** Description: Gets the data of all schedule item request status records in the database.
   /** Out: p_results_cursor - Cursor containing the data for all schedule item request statuses.
   /*********************************************************************************************************/
   PROCEDURE sp_get_all_sched_item_req_stat
   (
      p_results_cursor        OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_sched_item_req_status;
   END sp_get_all_sched_item_req_stat;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_save_activity
   /** Description: Creates or updates an activity record depending on
   /**              whether record with id as p_activity_id exists or not.
   /** In: p_activity_id - The ID of the activity.
   /** In: p_activity_type_id - The ID of the activity type of the activity.
   /** In: p_default_start_time - The default start time of the activity.
   /** In: p_last_modified_dt - The last modified date.
   /** In: p_default_expire_days - The default expire days of the activity.
   /** In: p_last_modified_by - The user by whom it was last modified.
   /** In: p_work_day_amt - The work day amount of the activity.
   /** In: p_duration - The duration of the activity.
   /** In: p_activity_nm - The activity name of the activity.
   /** In: p_enabled_ind - Whether the activity is enabled(1) or disabled(0).
   /** In: p_activity_abbr - The abbreviation of the activity.
   /** In: p_exclusive_ind - Whether the activity is exclusive(1) or not(0).
   /*********************************************************************************************************/
   PROCEDURE sp_save_activity
   (
      p_activity_id           view_activity.ACTIVITY_ID%TYPE,
      p_activity_type_id      view_activity.ACTIVITY_TYPE_ID%TYPE,
      p_default_start_time    view_activity.DEFAULT_START_TIME%TYPE,
      p_last_modified_dt      view_activity.ACT_LAST_MODIFIED_DT%TYPE,
      p_default_expire_days   view_activity.DEFAULT_EXPIRE_DAYS%TYPE,
      p_last_modified_by      view_activity.ACT_LAST_MODIFIED_BY%TYPE,
      p_work_day_amt          view_activity.ACT_WORK_DAY_AMT%TYPE,
      p_duration              view_activity.ACT_DURATION%TYPE,
      p_activity_nm           view_activity.ACTIVITY_NM%TYPE,
      p_enabled_ind           view_activity.ENABLED_IND%TYPE,
      p_activity_abbr         view_activity.ACTIVITY_ABBR%TYPE,
      p_exclusive_ind         view_activity.EXCLUSIVE_IND%TYPE
   )
   IS
      numRecs   NUMBER;
   BEGIN
      SELECT COUNT(*) INTO numRecs
      FROM view_activity
      WHERE
         activity_id = p_activity_id;

      IF numRecs > 0
      THEN
         UPDATE view_activity
         SET
            activity_type_id = p_activity_type_id,
            default_start_time = p_default_start_time,
            act_last_modified_dt = p_last_modified_dt,
            default_expire_days = p_default_expire_days,
            act_last_modified_by = p_last_modified_by,
            act_work_day_amt = p_work_day_amt,
            act_duration = p_duration,
            activity_nm = p_activity_nm,
            enabled_ind = p_enabled_ind,
            activity_abbr = p_activity_abbr,
            exclusive_ind = p_exclusive_ind
         WHERE
            activity_id = p_activity_id;
      ELSE
         INSERT INTO view_activity
         (
            activity_id,
            activity_type_id,
            default_start_time,
            act_last_modified_dt,
            default_expire_days,
            act_last_modified_by,
            act_work_day_amt,
            act_duration,
            activity_nm,
            enabled_ind,
            activity_abbr,
            exclusive_ind
         )
         VALUES
         (
            p_activity_id,
            p_activity_type_id,
            p_default_start_time,
            p_last_modified_dt,
            p_default_expire_days,
            p_last_modified_by,
            p_work_day_amt,
            p_duration,
            p_activity_nm,
            p_enabled_ind,
            p_activity_abbr,
            p_exclusive_ind
         );
      END IF;
   END sp_save_activity;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_delete_activity
   /** Description: Deletes an activity record with id as p_activity_id.
   /** In: p_activity_id - The ID of the activity to delete.
   /** InOut: p_rows_affected - The number of rows deleted.
   /*********************************************************************************************************/
   PROCEDURE sp_delete_activity
   (
      p_activity_id         view_activity.ACTIVITY_ID%TYPE,
      p_rows_affected       IN OUT NUMBER
   )
   IS
   BEGIN
      SELECT COUNT(1) INTO p_rows_affected
      FROM view_activity
      WHERE activity_id = p_activity_id;

      DELETE FROM view_activity
      WHERE
         activity_id = p_activity_id;
   END sp_delete_activity;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_activity
   /** Description: Gets the data of an activity record with id as p_activity_id.
   /** In: p_activity_id - The ID of the activity.
   /** Out: p_results_cursor - Cursor containing the row data for the given id.
   /*********************************************************************************************************/
   PROCEDURE sp_get_activity
   (
      p_activity_id        view_activity.ACTIVITY_ID%TYPE,
      p_results_cursor     OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_activity
         WHERE
            activity_id = p_activity_id;
   END sp_get_activity;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_all_activities
   /** Description: Gets the data of all activity records in the database.
   /** In: p_hideDisabled - Whether to hide(1) or show(0) disabled activities.
   /** Out: p_results_cursor - Cursor containing the data for all schedule item request statuses.
   /*********************************************************************************************************/
   PROCEDURE sp_get_all_activities
   (
      p_hideDisabled       view_activity.ENABLED_IND%TYPE,
      p_results_cursor     OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_activity
         WHERE
            enabled_ind = 1 OR
            enabled_ind = p_hideDisabled;
   END sp_get_all_activities;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_save_sched_item
   /** Description: Creates or updates a schedule item record depending on
   /**              whether record with id as p_schedule_item_id exists or not.
   /** In: p_schedule_item_id - The ID of the schedule item.
   /** In: p_sched_request_status_id - The ID of the request status of the schedule item.
   /** In: p_sched_status_id - The ID of the status of the schedule item.
   /** In: p_note_id - The ID of the Hermes Generic Note of the schedule item.
   /** In: p_sched_role_note_id - The ID of the Hermes Generic Note of the schedule item.
   /** In: p_activity_id - The ID of the activity of the schedule item.   
   /** In: p_last_modified_dt - The last modified date.
   /** In: p_work_day_amt - The work day amount for the schedule item.
   /** In: p_last_modified_by - The user by whom it was last modified.
   /** In: p_version - The version of the schedule item.
   /** In: p_duration - The duration of the schedule item.
   /** In: p_exception_ind - The exception indicator of the schedule item.
   /** In: p_expire_dt - The expiration date of the schedule item.
   /** In: p_work_dt - The work date of the schedule item.
   /*********************************************************************************************************/
   PROCEDURE sp_save_sched_item
   (
      p_schedule_item_id         view_schedule_item.SCHEDULE_ITEM_ID%TYPE,
      p_sched_request_status_id  view_schedule_item.SCHED_REQUEST_STATUS_ID%TYPE,
      p_sched_status_id          view_schedule_item.SCHED_STATUS_ID%TYPE,
      p_note_id                  view_schedule_item.NOTE_ID%TYPE,
      p_sched_role_note_id       view_schedule_item.SCHED_ROLE_NOTE_ID%TYPE,
      p_activity_id              view_schedule_item.ACTIVITY_ID%TYPE,
      p_last_modified_dt         view_schedule_item.LAST_MODIFIED_DT%TYPE,
      p_work_day_amt             view_schedule_item.WORK_DAY_AMT%TYPE,
      p_last_modified_by         view_schedule_item.LAST_MODIFIED_BY%TYPE,
      p_version                  view_schedule_item.VERSION%TYPE,
      p_duration                 view_schedule_item.DURATION%TYPE,
      p_exception_ind            view_schedule_item.EXCEPTION_IND%TYPE,
      p_expire_dt                view_schedule_item.EXPIRE_DT%TYPE,
      p_work_dt                  view_schedule_item.WORK_DT%TYPE
   )
   IS
      numRecs   NUMBER;
   BEGIN
      SELECT COUNT(*) INTO numRecs
      FROM view_schedule_item
      WHERE
         schedule_item_id = p_schedule_item_id;

      IF numRecs > 0
      THEN
         UPDATE view_schedule_item
         SET
            sched_request_status_id = p_sched_request_status_id,
            sched_status_id = p_sched_status_id,
            note_id = p_note_id,
            sched_role_note_id = p_sched_role_note_id,
            activity_id = p_activity_id,
            last_modified_dt = p_last_modified_dt,
            work_day_amt = p_work_day_amt,
            last_modified_by = p_last_modified_by,
            version = p_version,
            duration = p_duration,
            exception_ind = p_exception_ind,
            expire_dt = p_expire_dt,
            work_dt = p_work_dt
         WHERE
            schedule_item_id = p_schedule_item_id;
      ELSE
         INSERT INTO view_schedule_item
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
            p_schedule_item_id,
            p_sched_request_status_id,
            p_sched_status_id,
            p_note_id,
            p_sched_role_note_id,
            p_activity_id,
            p_last_modified_dt,
            p_work_day_amt,
            p_last_modified_by,
            p_version,
            p_duration,
            p_exception_ind,
            p_expire_dt,
            p_work_dt
         );
      END IF;
   END sp_save_sched_item;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_delete_sched_item
   /** Description: Deletes an schedule item record with id as p_schedule_item_id.
   /** In: p_schedule_item_id - The ID of the schedule item to delete.
   /** InOut: p_rows_affected - The number of rows deleted.
   /*********************************************************************************************************/
   PROCEDURE sp_delete_sched_item
   (
      p_schedule_item_id        view_schedule_item.SCHEDULE_ITEM_ID%TYPE,
      p_rows_affected           IN OUT NUMBER
   )
   IS
   BEGIN
      SELECT COUNT(1) INTO p_rows_affected
      FROM view_schedule_item
      WHERE schedule_item_id = p_schedule_item_id;

      DELETE FROM view_schedule_item
      WHERE
         schedule_item_id = p_schedule_item_id;
   END sp_delete_sched_item;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_sched_item
   /** Description: Gets the data of an schedule item record with id as p_schedule_item_id.
   /** In: p_schedule_item_id - The ID of the schedule item.
   /** Out: p_results_cursor - Cursor containing the row data for the given id.
   /*********************************************************************************************************/
   PROCEDURE sp_get_sched_item
   (
      p_schedule_item_id        view_schedule_item.SCHEDULE_ITEM_ID%TYPE,
      p_results_cursor          OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_schedule_item
         WHERE
            schedule_item_id = p_schedule_item_id;
   END sp_get_sched_item;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_all_sched_items
   /** Description: Gets the data of all schedule item records in the database.
   /** Out: p_results_cursor - Cursor containing the data for all schedule items.
   /*********************************************************************************************************/
   PROCEDURE sp_get_all_sched_items
   (
      p_results_cursor     OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_schedule_item;
   END sp_get_all_sched_items;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_relate_sched_items
   /** Description: Forms a parent - edit copy  relation between 2 schedule items.
   /** In: p_published_schedule_item_id - The parent schedule item.
   /** In: p_edit_schedule_item_id - The edit copy schedule item.
   /** In: p_last_modified_dt - The last modification date of this relation.
   /** In: p_last_modified_by - The user who performed the last modification.
   /*********************************************************************************************************/
   PROCEDURE sp_relate_sched_items
   (
      p_published_schedule_item_id     view_publish_xref.PUBLISHED_SCHEDULE_ITEM_ID%TYPE,
      p_edit_schedule_item_id          view_publish_xref.EDIT_SCHEDULE_ITEM_ID%TYPE,
      p_last_modified_dt               view_publish_xref.LAST_MODIFIED_DT%TYPE,
      p_last_modified_by               view_publish_xref.LAST_MODIFIED_BY%TYPE
   )
   IS
      numRecs   NUMBER;
   BEGIN
      SELECT COUNT(1) INTO numRecs FROM view_publish_xref
      WHERE
         published_schedule_item_id = p_published_schedule_item_id OR
         edit_schedule_item_id = p_edit_schedule_item_id;

      IF numRecs = 0
      THEN
         INSERT INTO view_publish_xref
         (
            sched_item_publish_xref_ID,
            published_schedule_item_ID,
            edit_schedule_item_ID,
            last_modified_by,
            last_modified_dt
         )
         VALUES
         (
            sys_guid(),
            p_published_schedule_item_id ,
            p_edit_schedule_item_id,
            p_last_modified_by,
            p_last_modified_dt
         );
      END IF;
   END sp_relate_sched_items;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_delete_sched_item_relation
   /** Description: Deletes a parent - edit copy relation between 2 schedule items.
   /** In: p_edit_schedule_item_id - The id of the edit copy schedule item for which to delete the relation.
   /*********************************************************************************************************/
   PROCEDURE sp_delete_sched_item_relation
   (
      p_edit_schedule_item_id       view_publish_xref.EDIT_SCHEDULE_ITEM_ID%TYPE
   )
   IS
   BEGIN
      DELETE FROM view_publish_xref
      WHERE
         edit_schedule_item_id = p_edit_schedule_item_id;
   END sp_delete_sched_item_relation;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_sched_item_edit_copy
   /** Description: Gets the edit copy for a given parent schedule item.
   /** In: p_published_schedule_item_id - The id of the parent schedule item for which to get the edit copy.
   /*********************************************************************************************************/
   PROCEDURE sp_get_sched_item_edit_copy
   (
      p_published_schedule_item_id  view_publish_xref.PUBLISHED_SCHEDULE_ITEM_ID%TYPE,
      p_results_cursor              OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_schedule_item
         WHERE
            schedule_item_ID =
            (SELECT edit_schedule_item_ID FROM view_publish_xref
             WHERE
               published_schedule_item_id = p_published_schedule_item_id);
   END sp_get_sched_item_edit_copy;



   /*********************************************************************************************************
   /**
   /** Procedure: sp_get_sched_item_parent
   /** Description: Gets the parent for a given edit copy schedule item.
   /** In: p_published_schedule_item_id - The id of the edit copy schedule item for which to get the parent.
   /*********************************************************************************************************/
   PROCEDURE sp_get_sched_item_parent
   (
      p_edit_schedule_item_ID    view_publish_xref.EDIT_SCHEDULE_ITEM_ID%TYPE,
      p_results_cursor           OUT T_CURSOR_TYPE
   )
   IS
   BEGIN
      OPEN p_results_cursor FOR
         SELECT * FROM view_schedule_item
         WHERE
            schedule_item_ID =
            (SELECT published_schedule_item_id FROM view_publish_xref
             WHERE
               edit_schedule_item_ID = p_edit_schedule_item_ID);
   END sp_get_sched_item_parent;


END schedule_item;
/
