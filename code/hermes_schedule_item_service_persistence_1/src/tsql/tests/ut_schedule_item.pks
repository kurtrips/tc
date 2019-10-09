/**************************************************************************************************
/*
/* Filename: ut_schedule_item.pks
/* Component: Hermes Schedule Item Persistence
/* Designer: argolite
/* Developer: TCSDeveloper
/* Version: 1.0
/* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
/*
/* Description: The specification of unit tests for the stored procedures present in the component.
/*
/**************************************************************************************************/

CREATE OR REPLACE PACKAGE ut_schedule_item
IS
   /*********************************************************************************************************
   /** Description: This is an equivalent of typedef.
   /*********************************************************************************************************/
   TYPE T_CURSOR_TYPE IS REF CURSOR;

   /*********************************************************************************************************
   /**
   /** Procedure: ut_setup
   /** Description: UtPLSQL calls this before all tests
   /*********************************************************************************************************/
   PROCEDURE ut_setup;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_teardown
   /** Description: UtPLSQL calls this after all tests. Clears data from all tables.
   /*********************************************************************************************************/   
   PROCEDURE ut_teardown;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_ctvty_grp
   /** Description: Unit Test for the sp_save_activity_group procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_save_ctvty_grp;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_delete_ctvty_grp
   /** Description: Unit Test for the sp_delete_activity_group procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_delete_ctvty_grp;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_ctvty_grp
   /** Description: Unit Test for the sp_get_activity_group procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_ctvty_grp;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_cvty_grps
   /** Description: Unit Test for the sp_get_all_activity_groups procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_all_cvty_grps;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_ctvty_type
   /** Description: Unit Test for the sp_save_activity_type procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_save_ctvty_type;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_delete_ctvty_type
   /** Description: Unit Test for the sp_delete_activity_type procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_delete_ctvty_type;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_ctvty_type
   /** Description: Unit Test for the sp_get_activity_type procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_ctvty_type;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_ctvty_types
   /** Description: Unit Test for the sp_get_all_activity_types procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_all_ctvty_types;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_schdl_item_stts
   /** Description: Unit Test for the sp_save_schedule_item_status procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_save_schdl_item_stts;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_delete_schdl_item_stts
   /** Description: Unit Test for the sp_delete_schedule_item_status procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_delete_schdl_item_stts;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_schdl_item_stts
   /** Description: Unit Test for the sp_get_schedule_item_status procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_schdl_item_stts;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_sched_item_stats
   /** Description: Unit Test for the sp_get_all_sched_item_stats procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_all_sched_item_stats;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_sched_item_req_stat
   /** Description: Unit Test for the sp_save_sched_item_req_stat procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_save_sched_item_req_stat;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_del_sched_item_req_stat
   /** Description: Unit Test for the sp_delete_sched_item_req_stat procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_del_sched_item_req_stat;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_sched_item_req_stat
   /** Description: Unit Test for the sp_get_sched_item_req_stat procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_sched_item_req_stat;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_sched_item_req_stats
   /** Description: Unit Test for the sp_get_all_sched_item_req_stat procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_sched_item_req_stats;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_ctvty
   /** Description: Unit Test for the sp_save_activity procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_save_ctvty;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_delete_ctvty
   /** Description: Unit Test for the sp_delete_activity procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_delete_ctvty;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_ctvty
   /** Description: Unit Test for the sp_get_activity procedure
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_ctvty;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_activities
   /** Description: Unit Test for the sp_get_all_activities procedure when showing disabled records.
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_all_activities;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_activities1
   /** Description: Unit Test for the sp_get_all_activities procedure when hiding disabled records.
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_all_activities1;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_sched_item
   /** Description: Unit Test for the sp_save_sched_item procedure.
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_save_sched_item;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_delete_sched_item
   /** Description: Unit Test for the sp_save_sched_item procedure.
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_delete_sched_item;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_sched_item
   /** Description: Unit Test for the sp_get_sched_item procedure.
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_sched_item;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_sched_items
   /** Description: Unit Test for the sp_get_all_sched_items procedure.
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_all_sched_items;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_relate_sched_items
   /** Description: Unit Test for the sp_relate_sched_items procedure.
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_relate_sched_items;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_del_sched_item_relation
   /** Description: Unit Test for the sp_delete_sched_item_relation procedure.
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_del_sched_item_relation;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_sched_item_edit_copy
   /** Description: Unit Test for the sp_get_sched_item_edit_copy procedure.
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_sched_item_edit_copy;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_sched_item_parent
   /** Description: Unit Test for the sp_get_sched_item_parent procedure.
   /*********************************************************************************************************/   
   PROCEDURE ut_sp_get_sched_item_parent;
END ut_schedule_item;
/