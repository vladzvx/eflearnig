drop table "DataLogs";

create table "DataLogs"
(
    "MainId"      uuid   not null,
    "ParentMainId"      uuid ,
    "PartitionId" bigint not null,
    "ParentPartitionId" bigint,
    "Payload"     text,
    "Entity1Id"   bigint
        constraint "FK_DataLogs_Entity1s_Entity1Id"
            references "Entity1s",
    "Entity2Id"   text
        constraint "FK_DataLogs_Entity2s_Entity2Id"
            references "Entity2s",
    primary key  ("MainId","PartitionId"),
    foreign key ("ParentMainId","ParentPartitionId") references "DataLogs"("MainId","PartitionId")

) partition by list ("PartitionId");

CREATE TABLE DataLogs0 PARTITION OF "DataLogs" FOR VALUES IN (0);
CREATE TABLE DataLogs1 PARTITION OF "DataLogs" FOR VALUES IN (1);
CREATE TABLE DataLogs2 PARTITION OF "DataLogs" FOR VALUES IN (2);

alter table "DataLogs"
    owner to postgres;


create index "IX_DataLogs_Entity1Id"
    on "DataLogs" ("Entity1Id");

create index "IX_DataLogs_Entity2Id"
    on "DataLogs" ("Entity2Id");

select * from DataLogs0;
select * from DataLogs1;
select * from DataLogs2;

create index "DataLogs0_index" on DataLogs0("Payload");

explain select "Payload" from "DataLogs" where "Payload"='qw';
explain select "Payload" from "DataLogs" where "PartitionId"=0 and  "Payload"='qw';
explain select "Payload" from "DataLogs" where "PartitionId"=1 and  "Payload"='qw';

update "DataLogs" set "PartitionId"=1 where "MainId"='4434eb80-fe01-4982-a46c-231cfd870928';
