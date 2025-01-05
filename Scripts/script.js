// Require fs module for file operations
const fs = require('fs');

// Require util module to promisify setTimeout
const util = require('util');
const sleep = util.promisify(setTimeout);

// Define the output file path
const outputFile = `${process.env.HOME}/scripts/output.txt`;

//if file doesn't exist , create it
if (!fs.existsSync(outputFile)) {
    fs.writeFileSync(outputFile, '');
}

// Get current datetime and write to output file
fs.appendFileSync(outputFile,new Date().toString()+'node1');

// Wait for 5 seconds
sleep(5000).then(() => {
    // Append current datetime to output file
    fs.appendFileSync(outputFile, '\n' + new Date().toString() + 'node2');
});

