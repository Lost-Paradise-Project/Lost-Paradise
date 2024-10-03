@echo off

call pip install -r requirements.txt
call python3 ./yamlextractor.py
call python3 ./keyfinder.py
call python3 ./clean_ftl.py
call python3 ./1.py
