insert into activity_group values(
    '11111111111111111111111111111111', '20-NOV-2007', 'ivern', 'DEV', 'Development');
insert into activity_type values(
    '22222222222222222222222222222222', '11111111111111111111111111111111', '20-NOV-2007',
    'Post Dev Dists', 'assistant', 'PDD');
insert into ACTIVITY
  (ACTIVITY_ID, ACTIVITY_TYPE_ID, DEFAULT_START_TIME, LAST_MODIFIED_DT,
   DEFAULT_EXPIRE_DAYS, LAST_MODIFIED_BY, WORK_DAY_AMT, DURATION, ACTIVITY_NM,
   ENABLED_IND, ACTIVITY_ABBR, EXCLUSIVE_IND)
values
    ('33333333333333333333333333333333', '22222222222222222222222222222222',
     '20-NOV-2007', '20-NOV-2007', 12, 'jhughes', 34, 56, 'Run TC', 1, 'RTC', 1);
insert into SCHED_STATUS
  (SCHED_STATUS_ID, LAST_MODIFIED_DT, LAST_MODIFIED_BY, STATUS_ABBR, STATUS_DESC)
values
  ('4444444444444444444444444444444A', '20-NOV-2007', 'petr', 'EDC', 'edit copy');
insert into SCHED_STATUS
  (SCHED_STATUS_ID, LAST_MODIFIED_DT, LAST_MODIFIED_BY, STATUS_ABBR, STATUS_DESC)
values
  ('4444444444444444444444444444444B', '20-NOV-2007', 'petr', 'RET', 'retired');
insert into SCHED_STATUS
  (SCHED_STATUS_ID, LAST_MODIFIED_DT, LAST_MODIFIED_BY, STATUS_ABBR, STATUS_DESC)
values
  ('44444444444444444444444444444444', '20-NOV-2007', 'petr', 'PUB', 'published');  
insert into SCHED_REQUEST_STATUS
  (SCHED_REQUEST_STATUS_ID, LAST_MODIFIED_DT, LAST_MODIFIED_BY, STATUS_DESC,
   STATUS_ABBR)
values
  ('55555555555555555555555555555555', '20-NOV-2007', 'sparemax', 'Wow are you strict or what!', 'WOW');
insert into SCHED_ITEM
  (SCHEDULE_ITEM_ID, SCHED_REQUEST_STATUS_ID, SCHED_STATUS_ID, NOTE_ID,
   SCHED_ROLE_NOTE_ID, ACTIVITY_ID, LAST_MODIFIED_DT, WORK_DAY_AMT, LAST_MODIFIED_BY,
   VERSION, DURATION, EXCEPTION_IND, EXPIRE_DT, WORK_DT)
values
  ('66666666666666666666666666666666', '55555555555555555555555555555555', '44444444444444444444444444444444',
  null, null, '33333333333333333333333333333333',
  '20-NOV-2007', 23, 'cnettel', 1, 13, 1, '20-NOV-2007', '20-NOV-2007');
insert into SCHED_ITEM
  (SCHEDULE_ITEM_ID, SCHED_REQUEST_STATUS_ID, SCHED_STATUS_ID, NOTE_ID,
   SCHED_ROLE_NOTE_ID, ACTIVITY_ID, LAST_MODIFIED_DT, WORK_DAY_AMT, LAST_MODIFIED_BY,
   VERSION, DURATION, EXCEPTION_IND, EXPIRE_DT, WORK_DT)
values
  ('6666666666666666666666666666666A', '55555555555555555555555555555555', '4444444444444444444444444444444A',
  null, null, '33333333333333333333333333333333',
  '20-NOV-2007', 23, 'cnettel', 1, 13, 1, '20-NOV-2007', '20-NOV-2007');
insert into SCHED_ITEM
  (SCHEDULE_ITEM_ID, SCHED_REQUEST_STATUS_ID, SCHED_STATUS_ID, NOTE_ID,
   SCHED_ROLE_NOTE_ID, ACTIVITY_ID, LAST_MODIFIED_DT, WORK_DAY_AMT, LAST_MODIFIED_BY,
   VERSION, DURATION, EXCEPTION_IND, EXPIRE_DT, WORK_DT)
values
  ('6666666666666666666666666666666B', '55555555555555555555555555555555', '4444444444444444444444444444444B',
  null, null, '33333333333333333333333333333333',
  '20-NOV-2007', 23, 'cnettel', 1, 13, 1, '20-NOV-2007', '20-NOV-2007');
COMMIT