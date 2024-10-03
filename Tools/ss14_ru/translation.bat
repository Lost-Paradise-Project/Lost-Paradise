@echo off

call pip install -r requirements.txt
call py ./yamlextractor.py
call py ./keyfinder.py
call py ./clean_ftl.py
call py ./1.py
