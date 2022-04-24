# 5. Data persistance

## Design a data schema for a database (PostgreSQL)

```

CREATE SEQUENCE public."Tree_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 9223372036854775807
    CACHE 1;
	
CREATE TABLE public."Tree"
( 
    "Id" integer NOT NULL DEFAULT nextval('"Tree_id_seq"'::regclass),
    "Label" character varying(64) NOT NULL,
	"ParentId" integer,
    CONSTRAINT "PK_Tree" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Tree_ParentId" FOREIGN KEY ("ParentId")
        REFERENCES public."Tree" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

```

## Entity Framework 

```
using (EFTreeContext db = new EFTreeContext()) // step 1 Context (db) is created
{
  // Entity (tree node) is added to the context
  Tree root = new Tree(){ Id = "1", Label = "root"};
  db.Tree.Add(root);
 
  Tree ant = new Tree(){ Id = "2", Label = "ant", ParentId = 1};
  db.Tree.Add(ant);
  
  Tree bear = new Tree(){ Id = "3", Label = "bear", ParentId = 1};
  db.Tree.Add(bear);
  
  Tree frog = new Tree(){ Id = "7", Label = "frog", ParentId = 1};
  db.Tree.Add(frog);
  
  Tree cat = new Tree(){ Id = "4", Label = "cat", ParentId = 3};
  db.Tree.Add(cat);
  
  Tree dog = new Tree(){ Id = "5", Label = "dog", ParentId = 3};
  db.Tree.Add(dog);
  
  Tree elephant = new Tree(){ Id = "6", Label = "elephant", ParentId = 5};
  db.Tree.Add(elephant);
  
  db.SaveChanges(); // SaveChanges is called to persist data back to the database
}

```

# 6. Test

## 1. Query for getting all the tree data;

```

selelct * from "Tree";

```

## 2. Query for adding the node lion as child of bear node

```

INSERT INTO public."Tree"( "Label", "ParentId") VALUES ( "lion", 3);

```

## 3. Delete the node frog that does not have any children

```

 DELETE from "Tree" where "Id" = 7 AND "Id" Not in (select "ParentId" from "Tree" where "ParentId" = 7);

```

## 4. Update the node bear with the new Id 11 and add node bear as Child to the node with Id 11

```
-- create the node with Id 11
INSERT INTO public."Tree"( "Label", "ParentId") VALUES ( 'bear', 1);

-- update all the node with the new parent Id 11
UPDATE public."Tree"
	SET "ParentId"=11
	WHERE "ParentId" = 3;
	
-- insert the node bear that has parent Id 11
INSERT INTO public."Tree"( "Label", "ParentId") VALUES ( 'bear', 11);

DELETE  from "Tree" where "Id" = 3;

```



