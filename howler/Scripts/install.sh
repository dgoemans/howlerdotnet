#!/bin/sh

cd ..

#copy the libs into the req'd dir

cp Scripts/libqyotoshared.so.1.0.0 .
cp Scripts/libqyoto.so .
cp Scripts/libsmokeqt.so.2.0.0 .

# create sym links

ln -sTf libqyotoshared.so.1.0.0 libqyotoshared.so.1
ln -sTf libqyotoshared.so.1.0.0 libqyotoshared.so

ln -sTf libsmokeqt.so.2.0.0 libsmokeqt.so.2
ln -sTf libsmokeqt.so.2.0.0 libsmokeqt.so