#!/bin/bash
set -e

MONGOSH=(mongosh --host mongodb --quiet --username admin --password "$ADMIN_PASSWORD" --authenticationDatabase admin)

until "${MONGOSH[@]}" --eval "db.adminCommand('ping')" >/dev/null 2>&1; do
  sleep 2
done

"${MONGOSH[@]}" --eval '
  try {
    rs.status();
  } catch (error) {
    rs.initiate({
      _id: "rs0",
      members: [{ _id: 0, host: "mongodb:27017" }]
    });
  }
'

until "${MONGOSH[@]}" --quiet --eval "rs.isMaster().ismaster" | grep -q true; do
  sleep 2
done

"${MONGOSH[@]}" --eval '
  const adminDb = db.getSiblingDB("admin");
  if (!adminDb.getUser("mongotUser")) {
    adminDb.createUser({
      user: "mongotUser",
      pwd: process.env.MONGOT_PASSWORD,
      roles: [{ role: "searchCoordinator", db: "admin" }]
    });
  }
'