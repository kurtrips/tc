/**********************************************************************
/*
/* Filename: schedule_item_views.sql
/* Component: Hermes Schedule Item Persistence
/* Designer: argolite
/* Developer: TCSDeveloper
/* Version: 1.0
/* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
/*
/* Description: The views on the various tables for this component.
/*
/**********************************************************************/



/*****************************************************************************************************************
/**
/** View: view_activity_group
/** Description: A view on activity_group for performing CRUD operations on activity_group table
/*****************************************************************************************************************/
CREATE OR REPLACE VIEW view_activity_group AS
SELECT
  activity_group.activity_group_ID, 
  activity_group.last_modified_dt AS act_grp_last_modified_dt,
  activity_group.last_modified_by AS act_grp_last_modified_by,
  activity_group.act_grp_abbr,
  activity_group.act_grp_nm
FROM activity_group;



/*****************************************************************************************************************
/**
/** View: view_sched_item_req_status
/** Description: A view on sched_request_status for performing CRUD operations on sched_request_status table
/*****************************************************************************************************************/
CREATE OR REPLACE VIEW view_sched_item_req_status AS
SELECT
  sched_request_status.sched_request_status_ID,
  sched_request_status.last_modified_by AS rs_last_modified_by,
  sched_request_status.last_modified_dt AS rs_last_modified_dt,
  sched_request_status.status_abbr AS rs_status_abbr,
  sched_request_status.status_desc AS rs_status_desc
FROM sched_request_status;



/*****************************************************************************************************************
/**
/** View: view_schedule_item_status
/** Description: A view on sched_status for performing CRUD operations on sched_status table
/*****************************************************************************************************************/
CREATE OR REPLACE VIEW view_schedule_item_status AS
SELECT
  sched_status.sched_status_ID,
  sched_status.last_modified_by AS s_last_modified_by,
  sched_status.last_modified_dt AS s_last_modified_dt,
  sched_status.status_abbr AS s_status_abbr,
  sched_status.status_desc AS s_status_desc
FROM sched_status;



/*****************************************************************************************************************
/**
/** View: view_activity_type
/** Description: A view on activity_type for performing CRUD operations on activity_type table
/*****************************************************************************************************************/
CREATE OR REPLACE VIEW view_activity_type AS
select
  activity_type.activity_type_ID, 
  activity_type.last_modified_by AS act_typ_last_modified_by,
  activity_type.last_modified_dt AS act_typ_last_modified_dt,
  activity_type.activity_type_abbr,
  activity_type.activity_type_nm,
  activity_type.activity_group_ID, 
  activity_group.last_modified_by AS act_grp_last_modified_by,
  activity_group.last_modified_dt AS act_grp_last_modified_dt,
  activity_group.act_grp_abbr,
  activity_group.act_grp_nm
from activity_type, activity_group
where activity_type.activity_group_ID = activity_group.activity_group_ID;




/*****************************************************************************************************************
/**
/** View: view_activity
/** Description: A view on activity for performing CRUD operations on activity table
/*****************************************************************************************************************/
CREATE OR REPLACE VIEW view_activity AS
SELECT
  activity.activity_ID,    
  activity.default_start_time,
  activity.activity_abbr,  
  activity.last_modified_dt AS act_last_modified_dt,
  activity.default_expire_days,
  activity.last_modified_by AS act_last_modified_by,
  activity.work_day_amt AS act_work_day_amt,
  activity.duration AS act_duration,
  activity.activity_nm,
  activity.exclusive_ind,
  activity.enabled_ind,
  view_activity_type.activity_type_ID, 
  view_activity_type.act_typ_last_modified_by,
  view_activity_type.act_typ_last_modified_dt,
  view_activity_type.activity_type_abbr,
  view_activity_type.activity_type_nm,
  view_activity_type.activity_group_ID, 
  view_activity_type.act_grp_last_modified_by,
  view_activity_type.act_grp_last_modified_dt,
  view_activity_type.act_grp_abbr,
  view_activity_type.act_grp_nm
FROM activity, view_activity_type
WHERE activity.activity_type_ID = view_activity_type.activity_type_ID;





/*****************************************************************************************************************
/**
/** View: view_schedule_item
/** Description: A view on sched_item for performing CRUD operations on sched_item table
/*****************************************************************************************************************/
CREATE OR REPLACE VIEW view_schedule_item AS
SELECT
  sched_item.schedule_item_ID,
  sched_item.sched_role_note_ID,
  sched_item.sched_request_status_ID,
  sched_item.sched_status_ID,
  sched_item.expire_dt,
  sched_item.last_modified_dt,
  sched_item.work_day_amt,
  sched_item.last_modified_by,
  sched_item.version,
  sched_item.exception_ind,
  sched_item.duration,
  sched_item.work_dt,
  sched_item.note_id,
  view_activity.activity_ID,    
  view_activity.default_start_time,
  view_activity.activity_abbr,  
  view_activity.act_last_modified_dt,
  view_activity.default_expire_days,
  view_activity.act_last_modified_by, 
  view_activity.act_work_day_amt,
  view_activity.act_duration,        
  view_activity.activity_nm,
  view_activity.exclusive_ind,
  view_activity.enabled_ind,
  view_activity.activity_type_ID, 
  view_activity.act_typ_last_modified_by,
  view_activity.act_typ_last_modified_dt,
  view_activity.activity_type_abbr,
  view_activity.activity_type_nm,
  view_activity.activity_group_ID, 
  view_activity.act_grp_last_modified_by,
  view_activity.act_grp_last_modified_dt,
  view_activity.act_grp_abbr,
  view_activity.act_grp_nm,
  view_sched_item_req_status.rs_last_modified_by,
  view_sched_item_req_status.rs_last_modified_dt,
  view_sched_item_req_status.rs_status_abbr,
  view_sched_item_req_status.rs_status_desc,
  view_schedule_item_status.s_last_modified_by,
  view_schedule_item_status.s_last_modified_dt,
  view_schedule_item_status.s_status_abbr,
  view_schedule_item_status.s_status_desc
FROM   sched_item, view_sched_item_req_status, view_schedule_item_status, view_activity
WHERE  sched_item.activity_ID = view_activity.activity_ID AND
	   sched_item.sched_request_status_ID = view_sched_item_req_status.sched_request_status_ID AND
	   sched_item.sched_status_ID = view_schedule_item_status.sched_status_ID;
	
	
	

/*****************************************************************************************************************
/**
/** View: view_publish_xref
/** Description: A view on sched_item_publish_xref for performing CRUD operations on sched_item_publish_xref table
/*****************************************************************************************************************/
CREATE OR REPLACE VIEW view_publish_xref AS
SELECT
  sched_item_publish_xref.sched_item_publish_xref_ID,
  sched_item_publish_xref.published_schedule_item_ID,
  sched_item_publish_xref.edit_schedule_item_ID,
  sched_item_publish_xref.last_modified_by,
  sched_item_publish_xref.last_modified_dt
FROM sched_item_publish_xref;

