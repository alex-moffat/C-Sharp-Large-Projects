# C-Sharp-Large-Projects

## LIVE PROJECT
I had an opportunity to work on a two-week spring of a large-scale ASP .NET MVC website with 6 other developers. Within a couple days I was able to get up speed on a large existing codebase from multiple prior contributors that was being altered to satisfy future roadmap specifications. During my time on the project I seamlessly merged code that made significant improvements to a large existing web application without comprising legacy functionality. During the two-week project I completed five stories that improved back-end functionality and front-end design while identifying and fixing existing bugs. Even during the short span of time, I was able to connect and collaborate with team members to help accelerate their stories.

### Description
This project is built using ASP .Net MVC and Entity Framework. This project is the interactive website for managing the content and productions for a theater/acting company. It's meant to be a content management service (aka CMS) for users who are not technically saavy and want to easily manage what displays in their website. It's also meant to help manage login capability for subscribers, and maintain a wiki of past performances and performers. 

## CONTENT
-  (Align-Checkboxes)
- [Story 2] (Replace-static-images-on-home-page-with-carousels)

## STORIES
### Story 1: Align Checkboxes
-	Align Checkboxes on CastMember Create Page
-	Create three responsive columns
-	Center checkboxes and labels in columns
-	Remove focus highlight from checkbox container border
-	Center labels for CastYearLeft and DebutYear

### Story 2: Replace static images on home page with carousels 
-	Create carousel for each production that automatically rotates through all available Production Photos.
-	Randomize production photo order
-	Rotate every 10 seconds.
-	Transition animation is smooth.
-	Maintain the responsiveness of the page, images, and production content.
-	Maintain style for production content and image overlay effects
-	Reduce code base by 200+ lines
- Identify and comment 250+ obsolete code

### Story 3: Emulate part details card with part create form 
-	Change the Part Create page so admin knows what the Part card is going to look like as they are creating it.  
-	Parts on the Part Index page are contained in red cards.  
-	On the Part Create page, create a card like the ones on the Part Index page.  
-	Near the top of the card create a dropdown for the Cast Member’s name.  
-	Create label "Playing" followed by a text box for character entry.  
-	Create label "In the Production" followed by a dropdown with a list of Productions.  
-	Create label "Details" followed by a textbox where the admin can type in the details.  
-	Below the card, there is a submit button.  
-	Above the card, the text "Creating a new Part" and in-between "new" and "Part" a dropdown for Part type.
-	Swap the positions of the Cast Member text and the Character text on the card so that the Cast Member's name is the title of the Part cards on index page.  
-	Change the "Performed By" text to "Playing".  
-	Fix the character field to display the character that the Cast Member is playing.
-	Prevent duplicate parts from being created when create is pressed with no cast member or production being selected.
-	Add validation to Part Create Page to check for cast member and production selection.
-	Remove “character” input field, assign label to cast member name and change style with all non-Actor Part Types. 

### Story 4: Enable photo update during Production creation  
-	Create an input field in the Productions Create page that allows the admin to be able to select a photo from their file system.
-	When the Create form submits, new Photo and Production Photo records are created
- The Production's DefaultPhoto field is set to new ProductionPhoto Id.
-	File input placed below the Closing Day input field.
-	<b>Create validation method to check if uploaded file is a photo file type</b>
-	Make name field required for production creation
-	Name the new photo in Photos DB the production name and make details of production photo “Default Photo”
-	Add photo validation feedback to user that upload file is not a valid photo and allow new selection before create.
-	Allow Create production without photo if no upload file is given
-	Use selected Season after failed validation and current season on new page load 

### Story 5: Add custom validation & fix display bug
-	Prevent the creation of Productions with null Evening and Matinee showtimes.  
-	Fix error on the Details page of Production will not display a production with null showtimes.  
-	Validate that at least one showtime is provided to allow saving a new production.
-	Create validation messages "At least one showtime must be specified" on the Productions Create page if the user attempts to create a Production where Evening and Matinee showtimes are both null.
-	Ensure that users can navigate to the Productions Details page without throwing an error if there are null Matinee or Evening showtimes.
-	Display “Not Playing” on the Productions details page for null Evening and Matinee showtimes. 


