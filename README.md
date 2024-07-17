# TeboCam_OpenSource

Please note that you will also need to download the PulseCheck application from https://github.com/guythiebaut/PulseCheck
This is a heart beat application that will restart Tebocam if Tebocam freezes.

Once you have downloaded and built PulseCheck, copy the binaries from the bin folder of PulseCheck into the relevant (debug/release) bin folder of the Tebocam application.
Note - You will need to build the Tebocam application first to create the Tebocam bin directories.

You can also get the PulseCheck application from Nuget by running the following from the command line:
````
nuget install TeboWeb-PulseCheck
````
