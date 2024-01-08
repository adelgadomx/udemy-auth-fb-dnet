CREATE OR REPLACE FUNCTION fx_query_productos(id int4 DEFAULT null)
RETURNS SETOF "Productos" AS
$$
BEGIN
  RETURN QUERY
  SELECT "Id","Nombre","Descripcion", "Precio"  
    FROM "Productos" 
   WHERE "Id" = COALESCE(id, "Id");
END
$$
LANGUAGE plpgsql;
SELECT * FROM fx_query_productos() order by "Id" desc;
SELECT * FROM fx_query_productos(501);
________________________________________________________________________________________________
;
CREATE OR REPLACE FUNCTION fx_query_productos_by_nombre(in _nombre text)
RETURNS TABLE(id int4, nombre text, descripcion text, precio numeric) AS
$$
BEGIN
  RETURN QUERY
    SELECT "Id","Nombre","Descripcion", "Precio" 
      FROM "Productos" 
     WHERE LOWER("Nombre") LIKE '%' || LOWER(_nombre) || '%';
END;
$$
LANGUAGE plpgsql;
SELECT * FROM fx_query_productos_by_nombre('licensed');
________________________________________________________________________________________________
;
CREATE OR REPLACE PROCEDURE sp_insert_producto(_precio numeric, _nombre text, _descripcion text)
LANGUAGE SQL
AS $BODY$
  INSERT INTO "Productos"("Precio", "Nombre", "Descripcion")
  VALUES(_precio, _nombre, _descripcion);
$BODY$;

CALL sp_insert_producto(100.11, 'Macbook Vaxi', 'Computadora de alta gama');
SELECT * FROM fx_query_productos_by_nombre('Vaxi');
________________________________________________________________________________________________
;
CREATE OR REPLACE PROCEDURE sp_update_producto(_id int4, _precio numeric, _nombre text, _descripcion text)
LANGUAGE SQL
AS $BODY$

  UPDATE "Productos" SET "Precio"=_precio, "Nombre"=_nombre, "Descripcion"=_descripcion WHERE "Id"=_id;   

$BODY$;
CALL sp_update_producto(1001, 500.23, 'IPhone Vaxi', 'Telefono inteligente' );
________________________________________________________________________________________________
;
CREATE OR REPLACE PROCEDURE sp_delete_producto(_id int4)
LANGUAGE SQL
AS $BODY$

  DELETE FROM "Productos" WHERE "Id"=_id;

$BODY$;
CALL sp_delete_producto(1001);
