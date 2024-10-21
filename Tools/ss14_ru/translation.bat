@echo off
pip install -r requirements.txt --no-warn-script-location
python ./yamlextractor.py
python ./keyfinder.py
python ./clean_ftl.py
python ./1.py

PAUSE
