# XMLTVImport
 Download and Import XMLTV files from xmltv.net directly into Windows Media Center
 
## What is XMLTVImport
XMLTVImport is a tool for importing TV Programmes in XMLTV format into Windows Media Center through the creation and import of a Windows Media Center Guide File (MXF).

## Configuration
1. Complete a Channel Scan in Windows Media Center.
2. Add the XMLTV file location as a URL in app.config.
3. Add the XMLTV Name and Location values in app.config (these will become the MXF Lineup Name and Short-name, respectively).
4. Add all Channel numbers (in format "callsign-{channelNumber}") & CallSign strings in app.config for automatic Channel Mapping.
5. Add all Channel icon locations as a URL in app.config (these will be downloaded to local storage on first run).
6. Run XMLTVImport and wait for the program to complete the MXF Import
7. Open Windows Media Center Guide and confirm that all Channels have mapped successfully.

## Troubleshooting
* If Windows Media Center does not display a logo or guide information for a Channel, confirm that the Channel has been assigned the correct Listing (**Edit Channel** > **Edit Listings**)
  * If there is no Listing available under **Edit Listings** that matches the Channel, confirm that your XMLTV file includes at least one programme for that channel.
