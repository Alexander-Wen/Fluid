This program takes a general template file, and a body file and merges it into a single functioning website.
go into the example folder and run Fluid.exe for an example of this program


HOW TO FORMAT template.tmpl FILE

insert >navigation< into wherever the code for a navbar should be

insert >content< into wherever the code for the content of the website should be


HOW TO FORMAT body.bd FILE

The structure of a body file is simple, each new page of the website is inside of a >body function. The >body function 
takes 2 arguments, the name of the webpage (What phrase or word will appear on the navigation bar), and the title of the 
webpage.

Inside of the >body function there currently exists 2 possible functions, >content and >picture. 

>content takes 1 argument, and that is the title of the section. Inside of the >content function is actual content of 
the section.

>picture takes the formatting of the picture as arguments, and then inside of the >picture function is the link to the picture
