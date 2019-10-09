/**********************************************************************
/*
/* Filename: ut_schedule_item.pkb
/* Component: Hermes Schedule Item Persistence
/* Designer: argolite
/* Developer: TCSDeveloper
/* Version: 1.0
/* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
/*
/* Description: The unit tests for the stored procedures present in the component.
/*
/**********************************************************************/

CREATE OR REPLACE PACKAGE BODY ut_schedule_item
IS
   /*********************************************************************************************************
   /**
   /** Procedure: ut_setup
   /** Description: UtPLSQL calls this before all tests
   /*********************************************************************************************************/
   PROCEDURE ut_setup
   IS
   BEGIN
      ut_teardown();
   END;

   /*********************************************************************************************************
   /**
   /** Procedure: ut_teardown
   /** Description: UtPLSQL calls this after all tests. Clears data from all tables.
   /*********************************************************************************************************/
   PROCEDURE ut_teardown
   IS
   BEGIN
      delete from sched_item_publish_xref;
      delete from sched_item_pub_xref_hist;
      delete from sched_item;
      delete from sched_item_hist;
      delete from activity;
      delete from activity_hist;
      delete from activity_type;
      delete from activity_type_hist;
      delete from activity_group;
      delete from activity_group_hist;
      delete from sched_status;
      delete from sched_status_hist;
      delete from sched_request_status;
      delete from sched_request_status_hist;
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_ctvty_grp
   /** Description: Unit Test for the sp_save_activity_group procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_save_ctvty_grp
   IS
      rows PLS_INTEGER;
   BEGIN
      select count(*) into rows from activity_group;
      utAssert.eq ('Must have 0 rows initially', rows, 0);
      
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      
      select count(*) into rows from activity_group;
      utAssert.eq ('Must have 1 row now', rows,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_delete_ctvty_grp
   /** Description: Unit Test for the sp_delete_activity_group procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_delete_ctvty_grp
   IS
      rows PLS_INTEGER;
      rowsAffected NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_delete_activity_group('12341234123412341234123412341234', rowsAffected);

      select count(*) into rows from activity_group;
      utAssert.eq ('Row must now be deleted.', rows,  0);
      utAssert.eq ('Row must now be deleted.', rowsAffected,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_ctvty_grp
   /** Description: Unit Test for the sp_get_activity_group procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_ctvty_grp
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_activity_group%ROWTYPE;
      numRecords NUMBER;      
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_get_activity_group('12341234123412341234123412341234', dataCursor);
      numRecords := 0;
      
      open dataCursor for select * from view_activity_group;
      schedule_item.sp_get_activity_group('12341234123412341234123412341234', dataCursor);
      
      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;      
      utAssert.eq ('Wrong amount of data.', numRecords, 1);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_cvty_grps
   /** Description: Unit Test for the sp_get_all_activity_groups procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_all_cvty_grps
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_activity_group%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc0', 'def0', 'efg0');
      schedule_item.sp_save_activity_group('76547654765476547654765476547654', SYSDATE, 'abc1', 'def1', 'efg1');
      numRecords := 0;
      
      open dataCursor for select * from view_activity_group;
      schedule_item.sp_get_all_activity_groups(dataCursor);
      
      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;      
      utAssert.eq ('Wrong amount of data.', numRecords, 2);

      ut_teardown();
   END;

   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_ctvty_type
   /** Description: Unit Test for the sp_save_activity_type procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_save_ctvty_type
   IS
      rows PLS_INTEGER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      
      select count(*) into rows from activity_type;
      utAssert.eq ('Must have 0 rows initially', rows, 0);
      
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      
      select count(*) into rows from activity_type;
      utAssert.eq ('Must have 1 row now', rows,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_delete_ctvty_type
   /** Description: Unit Test for the sp_delete_activity_type procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_delete_ctvty_type
   IS
      rows NUMBER;
      rowsAffected NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');      
      schedule_item.sp_delete_activity_type('67896789678967896789678967896789', rowsAffected);

      select count(*) into rows from activity_type;
      utAssert.eq ('Row must now be deleted.', rows,  0);
      utAssert.eq ('Row must now be deleted.', rowsAffected,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_ctvty_type
   /** Description: Unit Test for the sp_get_activity_type procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_ctvty_type
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_activity_type%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      numRecords := 0;

      open dataCursor for select * from view_activity_type;
      schedule_item.sp_get_activity_type('67896789678967896789678967896789', dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 1);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_ctvty_types
   /** Description: Unit Test for the sp_get_all_activity_types procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_all_ctvty_types
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_activity_type%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity_type('34093409340934093409340934093409',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      numRecords := 0;

      open dataCursor for select * from view_activity_type;
      schedule_item.sp_get_all_activity_types(dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 2);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_schdl_item_stts
   /** Description: Unit Test for the sp_save_schedule_item_status procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_save_schdl_item_stts
   IS
      rowCnt PLS_INTEGER;
   BEGIN
      select count(*) into rowCnt from sched_status;
      utAssert.eq ('Must have 0 rows initially', rowCnt, 0);

      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');

      select count(*) into rowCnt from sched_status;
      utAssert.eq ('Must have 1 row now', rowCnt,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_delete_schdl_item_stts
   /** Description: Unit Test for the sp_delete_schedule_item_status procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_delete_schdl_item_stts
   IS
      rowCnt NUMBER;
      rowsAffected NUMBER;
   BEGIN
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_delete_schedule_item_status('12341234123412341234123412341234', rowsAffected);

      select count(*) into rowCnt from sched_status;
      utAssert.eq ('Row must now be deleted.', rowCnt,  0);
      utAssert.eq ('Row must now be deleted.', rowsAffected,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_schdl_item_stts
   /** Description: Unit Test for the sp_get_schedule_item_status procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_schdl_item_stts
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_schedule_item_status%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      numRecords := 0;

      open dataCursor for select * from view_schedule_item_status;
      schedule_item.sp_get_schedule_item_status('12341234123412341234123412341234', dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 1);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_sched_item_stats
   /** Description: Unit Test for the sp_get_all_sched_item_stats procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_all_sched_item_stats
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_schedule_item_status%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc1', 'def1', 'efg1');
      schedule_item.sp_save_schedule_item_status('30983098309830983098309830983098', SYSDATE, 'abc1', 'def2', 'efg2');
      numRecords := 0;

      open dataCursor for select * from view_schedule_item_status;

      numRecords := 0;

      open dataCursor for select * from view_schedule_item_status;
      schedule_item.sp_get_all_sched_item_stats(dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 2);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_sched_item_req_stat
   /** Description: Unit Test for the sp_save_sched_item_req_stat procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_save_sched_item_req_stat
   IS
      rowCnt PLS_INTEGER;
   BEGIN
      select count(*) into rowCnt from sched_request_status;
      utAssert.eq ('Must have 0 rows initially', rowCnt, 0);

      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');

      select count(*) into rowCnt from sched_request_status;
      utAssert.eq ('Must have 1 row now', rowCnt,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_del_sched_item_req_stat
   /** Description: Unit Test for the sp_delete_sched_item_req_stat procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_del_sched_item_req_stat
   IS
      rowCnt NUMBER;
      rowsAffected NUMBER;
   BEGIN
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_delete_sched_item_req_stat('12341234123412341234123412341234', rowsAffected);

      select count(*) into rowCnt from sched_request_status;
      utAssert.eq ('Row must now be deleted.', rowCnt,  0);
      utAssert.eq ('Row must now be deleted.', rowsAffected,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_sched_item_req_stat
   /** Description: Unit Test for the sp_get_sched_item_req_stat procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_sched_item_req_stat
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_sched_item_req_status%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      numRecords := 0;

      open dataCursor for select * from view_sched_item_req_status;
      schedule_item.sp_get_sched_item_req_stat('12341234123412341234123412341234', dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 1);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_sched_item_req_stats
   /** Description: Unit Test for the sp_get_all_sched_item_req_stat procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_sched_item_req_stats
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_sched_item_req_status%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc1', 'def1', 'efg1');
      schedule_item.sp_save_sched_item_req_stat('30983098309830983098309830983098', SYSDATE, 'abc1', 'def2', 'efg2');
      numRecords := 0;

      open dataCursor for select * from view_sched_item_req_status;
      schedule_item.sp_get_all_sched_item_req_stat(dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 2);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_ctvty
   /** Description: Unit Test for the sp_save_activity procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_save_ctvty
   IS
      rowCnt PLS_INTEGER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');

      select count(*) into rowCnt from activity;
      utAssert.eq ('Must have 0 rows initially', rowCnt, 0);

      schedule_item.sp_save_activity(
         '12341234123412341234123412341234', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);

      select count(*) into rowCnt from activity;
      utAssert.eq ('Must have 1 row now', rowCnt,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_delete_ctvty
   /** Description: Unit Test for the sp_delete_activity procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_delete_ctvty
   IS
      rowCnt NUMBER;
      rowsAffected NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_delete_activity('34323432343234323432343234323432', rowsAffected);
      
      select count(*) into rowCnt from activity;
      utAssert.eq ('Row must now be deleted.', rowCnt,  0);
      utAssert.eq ('Row must now be deleted.', rowsAffected,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_ctvty
   /** Description: Unit Test for the sp_get_activity procedure
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_ctvty
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_activity%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      numRecords := 0;

      open dataCursor for select * from view_activity;
      schedule_item.sp_get_activity('34323432343234323432343234323432', dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 1);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_activities
   /** Description: Unit Test for the sp_get_all_activities procedure when showing disabled records.
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_all_activities
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_activity%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_save_activity(
         '90879087908790879087908790879087', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 1, 'efg', 1);
      numRecords := 0;

      open dataCursor for select * from view_activity;
      schedule_item.sp_get_all_activities(0, dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 2);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_activities1
   /** Description: Unit Test for the sp_get_all_activities procedure when hiding disabled records.
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_all_activities1
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_activity%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_save_activity(
         '90879087908790879087908790879087', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 1, 'efg', 1);
      numRecords := 0;

      open dataCursor for select * from view_activity;
      schedule_item.sp_get_all_activities(1, dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 1);

      ut_teardown();
   END;   
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_save_sched_item
   /** Description: Unit Test for the sp_save_sched_item procedure.
   /*********************************************************************************************************/
   PROCEDURE ut_sp_save_sched_item
   IS
      rowCnt NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');   

      select count(*) into rowCnt from sched_item;
      utAssert.eq ('Must have 0 rows initially', rowCnt, 0);

      schedule_item.sp_save_sched_item(
         '19191919191919191919191919191919', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);

      select count(*) into rowCnt from sched_item;
      utAssert.eq ('Must have 1 row now', rowCnt,  1);
      ut_teardown();      
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_delete_sched_item
   /** Description: Unit Test for the sp_save_sched_item procedure.
   /*********************************************************************************************************/
   PROCEDURE ut_sp_delete_sched_item
   IS
      rowsAffected NUMBER;
      rowCnt NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_sched_item(
         '19191919191919191919191919191919', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);      
      
      schedule_item.sp_delete_sched_item('19191919191919191919191919191919', rowsAffected);
            
      select count(*) into rowCnt from sched_item;
      utAssert.eq ('Row must now be deleted.', rowCnt,  0);
      utAssert.eq ('Row must now be deleted.', rowsAffected,  1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_sched_item
   /** Description: Unit Test for the sp_get_sched_item procedure.
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_sched_item
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_schedule_item%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_sched_item(
         '19191919191919191919191919191919', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);

      numRecords := 0;

      open dataCursor for select * from view_schedule_item;
      schedule_item.sp_get_sched_item('19191919191919191919191919191919', dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 1);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_all_sched_items
   /** Description: Unit Test for the sp_get_all_sched_items procedure.
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_all_sched_items
   IS
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_schedule_item%ROWTYPE;
      numRecords NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_sched_item(
         '19191919191919191919191919191919', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);
      schedule_item.sp_save_sched_item(
         '12981298129812981298129812981298', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);         

      numRecords := 0;

      open dataCursor for select * from view_schedule_item;
      schedule_item.sp_get_all_sched_items(dataCursor);

      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 2);

      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_relate_sched_items
   /** Description: Unit Test for the sp_relate_sched_items procedure.
   /*********************************************************************************************************/
   PROCEDURE ut_sp_relate_sched_items
   IS
      rowsCnt NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_sched_item(
         '19191919191919191919191919191919', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);
      schedule_item.sp_save_sched_item(
         '12981298129812981298129812981298', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);
         
      schedule_item.sp_relate_sched_items(
         '19191919191919191919191919191919', '12981298129812981298129812981298', SYSDATE, 'aip');
         
      select count(*) into rowsCnt from view_publish_xref;
      utAssert.eq ('Incorrect implementation.', rowsCnt, 1);
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_del_sched_item_relation
   /** Description: Unit Test for the sp_delete_sched_item_relation procedure.
   /*********************************************************************************************************/
   PROCEDURE ut_sp_del_sched_item_relation
   IS
      rowsCnt NUMBER;
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_sched_item(
         '19191919191919191919191919191919', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);
      schedule_item.sp_save_sched_item(
         '12981298129812981298129812981298', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);
         
      schedule_item.sp_relate_sched_items(
         '19191919191919191919191919191919', '12981298129812981298129812981298', SYSDATE, 'aip');
         
      select count(*) into rowsCnt from view_publish_xref;
      utAssert.eq ('Incorrect implementation.', rowsCnt, 1);
      
      schedule_item.sp_delete_sched_item_relation('12981298129812981298129812981298');
      
      select count(*) into rowsCnt from view_publish_xref;
      utAssert.eq ('Incorrect implementation.', rowsCnt, 0);      
      
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_sched_item_edit_copy
   /** Description: Unit Test for the sp_get_sched_item_edit_copy procedure.
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_sched_item_edit_copy
   IS
      rowsCnt NUMBER;
      numRecords NUMBER;
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_publish_xref%ROWTYPE;      
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_sched_item(
         '19191919191919191919191919191919', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);
      schedule_item.sp_save_sched_item(
         '12981298129812981298129812981298', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);
      schedule_item.sp_relate_sched_items(
         '19191919191919191919191919191919', '12981298129812981298129812981298', SYSDATE, 'aip');
         
      schedule_item.sp_get_sched_item_edit_copy('19191919191919191919191919191919', dataCursor);
      
      numRecords := 0;
      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 1);      
      
      ut_teardown();
   END;
   
   /*********************************************************************************************************
   /**
   /** Procedure: ut_sp_get_sched_item_parent
   /** Description: Unit Test for the sp_get_sched_item_parent procedure.
   /*********************************************************************************************************/
   PROCEDURE ut_sp_get_sched_item_parent
   IS
      rowsCnt NUMBER;
      numRecords NUMBER;
      dataCursor T_CURSOR_TYPE;
      tableRowRec view_publish_xref%ROWTYPE;      
   BEGIN
      schedule_item.sp_save_activity_group('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_activity_type('67896789678967896789678967896789',
         '12341234123412341234123412341234', 'abc1', SYSDATE, 'def', 'efg');
      schedule_item.sp_save_activity(
         '34323432343234323432343234323432', '67896789678967896789678967896789', SYSDATE, SYSDATE,
         17, 'abc', 24, 36, 'def', 0, 'efg', 1);
      schedule_item.sp_save_sched_item_req_stat('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_schedule_item_status('12341234123412341234123412341234', SYSDATE, 'abc', 'def', 'efg');
      schedule_item.sp_save_sched_item(
         '19191919191919191919191919191919', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);
      schedule_item.sp_save_sched_item(
         '12981298129812981298129812981298', '12341234123412341234123412341234', '12341234123412341234123412341234',
         NULL, NULL, '12341234123412341234123412341234', SYSDATE, 123, 'acd', 1, 2, 3, SYSDATE, SYSDATE);
      schedule_item.sp_relate_sched_items(
         '19191919191919191919191919191919', '12981298129812981298129812981298', SYSDATE, 'aip');
         
      schedule_item.sp_get_sched_item_parent('12981298129812981298129812981298', dataCursor);
      
      numRecords := 0;
      loop
         fetch dataCursor into tableRowRec;
         exit when dataCursor%notfound;
            numRecords := numRecords + 1;
      end loop;
      close dataCursor;
      utAssert.eq ('Wrong amount of data.', numRecords, 1);      
      
      ut_teardown();
   END;   
END ut_schedule_item;
/
