#!/bin/bash

# Define the output file path
output_file_path="$HOME/scripts/output.txt"

# Print the current datetime to the output file
date > "$output_file_path"

# Wait for 5 seconds
sleep 5

# Append the current datetime to the output file
date >> "$output_file_path"

