# C-Sharp-Large-Projects

## LIVE PROJECT
I had an opportunity to work on a two-week spring of a large-scale ASP .NET MVC website with 6 other developers. Within a couple days I was able to get up speed on a large existing codebase from multiple prior contributors that was being altered to satisfy future roadmap specifications. During my time on the project I seamlessly merged code that made significant improvements to a large existing web application without comprising legacy functionality. During the two-week project I completed five stories that improved back-end functionality and front-end design while identifying and fixing existing bugs. Even during the short span of time, I was able to connect and collaborate with team members to help accelerate their stories.

### Description
This project is built using ASP .Net MVC and Entity Framework. This project is the interactive website for managing the content and productions for a theater/acting company. It's meant to be a content management service (aka CMS) for users who are not technically saavy and want to easily manage what displays in their website. It's also meant to help manage login capability for subscribers, and maintain a wiki of past performances and performers. 

## CONTENT
- [Story 1: Align Checkboxes](#story-1-align-checkboxes)
- [Story 2: Replace static images on home page with carousels](#story-2-replace-static-images-on-home-page-with-carousels)
- [Story 3: Emulate part details card with part create form](#story-3-emulate-part-details-card-with-part-create-form)
- [Story 4: Enable photo update during Production creation](#story-4-enable-photo-update-during-production-creation)
- [Story 5: Add custom validation and fix display bug](#story-5-add-custom-validation-and-fix-display-bug)

## STORIES
### Story 1: Align Checkboxes
-	Align Checkboxes on CastMember Create Page
-	Create three responsive columns
-	Center checkboxes and labels in columns
-	Remove focus highlight from checkbox container border
-	Center labels for CastYearLeft and DebutYear

#### Before/After:
![alt text](https://github.com/alex-moffat/C-Sharp-Large-Projects/blob/master/CS_Story_1.jpg "Story_1")

#### CSHTML:
```HTML
<!--===== CHECKBOXES - match column width for group container =====-->
<div class="container col-md-10 text-center">
    <div class="row justify-content-center">
        <!--=== AssociateArtist ===-->
        <div class="form-group">
            @Html.LabelFor(model => model.AssociateArtist, htmlAttributes: new { @class = "control-label inputLabel" })
            <div class="focus-negate">
                @Html.EditorFor(model => model.AssociateArtist, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.AssociateArtist, "", new { @class = "text-danger" })
            </div>
        </div>
        <!--=== EnsembleMember ===-->
        <div class="mx-4 form-group">
            @Html.LabelFor(model => model.EnsembleMember, htmlAttributes: new { @class = "control-label inputLabel" })
            <div class="focus-negate">
                @Html.EditorFor(model => model.EnsembleMember, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.EnsembleMember, "", new { @class = "text-danger" })
            </div>
        </div>
        <!--=== CurrentMember ===-->
        <div class="form-group">
            @Html.LabelFor(model => model.CurrentMember, htmlAttributes: new { @class = "control-label inputLabel" })
            <div class="focus-negate">
                @Html.EditorFor(model => model.CurrentMember, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.CurrentMember, "", new { @class = "text-danger" })
            </div>
        </div>
    </div>
</div>
```
#### CSS:
```CSS
/*===== FOCUS NEGATE - selective remove focus highlight style from bootstrap form-control elements =====*/
.focus-negate .form-control:focus {
    border-color: none;
    box-shadow: none;
}
```

### Story 2: Replace static images on home page with carousels 
-	Create carousel for each production that automatically rotates through all available Production Photos.
-	Randomize production photo order
-	Rotate every 10 seconds.
-	Transition animation is smooth.
-	Maintain the responsiveness of the page, images, and production content.
-	Maintain style for production content and image overlay effects
-	Reduce code base by 200+ lines
-   Identify and comment 250+ obsolete code

#### Final:
![alt text](https://github.com/alex-moffat/C-Sharp-Large-Projects/blob/master/CS_Story_2.jpg "Story_2")

#### CS:
```CS
public ActionResult Index()
    {
        //==== LIST OF PRODUCTIONS - Filter by current and future productions
        var query = db.Productions.Where(p => p.OpeningDay > DateTime.Now || p.IsCurrent == true || (p.OpeningDay <= DateTime.Now && p.ClosingDay >= DateTime.Now))
            .OrderBy(p => p.OpeningDay);
        //===== ORDER PRODUCTIONS
        var orderedProductions = SortProductions(query.ToList());
        //===== PRODUCTION PICTURES - create list of productionPhotos objects for each production, randomize order of photo objects, nest productionPhotos objects list in a list, store list in ViewBag dictionary 
        var productionPhotosList = new List<List<ProductionPhotos>>();
        foreach (Production production in orderedProductions)
        {
            var photoArray = db.ProductionPhotos.Where(p => p.Production.ProductionId == production.ProductionId).ToList();
            productionPhotosList.Add(ShufflePhotos(photoArray));
            //DEBUG System.Diagnostics.Debug.WriteLine(photoArray.Count);
        }
        ViewBag.ProductionPhotosList = productionPhotosList;

        return View(orderedProductions);
    }
    
//===== SHUFFLE PHOTOS - takes a list of ProductionPhoto objects and shuffles them
private static readonly Random random = new Random();

private List<ProductionPhotos> ShufflePhotos(List<ProductionPhotos> photos)
    {
        int n = photos.Count;
        while (n > 1)
        {
            n--;
            int rnd = random.Next(n + 1);
            ProductionPhotos value = photos[rnd];
            photos[rnd] = photos[n];
            photos[n] = value;
        }
        return photos;
    }    
```


#### CSHTML:
```CSHTML
<div class="home-body">
    <main id="home-main">
        <!--========== MAIN CONTENT - production carousels and info ==========-->
        <div>
            <!--===== LOOP EACH PRODUCTION =====-->
            @foreach (var item in Model)
            {
                <!--===== PRODUCTION - Container for Carousel & Content =====-->
                <div class="home-block">
                    <div class="home-tint-overlay"></div>
                    <!--===== CAROUSEL =====-->
                    <div id="index-block-@i" class="carousel slide" data-interval="10000" data-ride="carousel">
                        <!--===== CAROUSEL INDICATORS -->
                        <ol class="carousel-indicators">
                            @{
                                <!--=== First slide -->
                                <li data-target="#index-block-@i" data-slide-to="@photoIndex" class="active"></li>
                                <!--=== More than one slide -->
                                if (productionPhotosList[productionIndex].Count > 1)
                                {
                                    photoIndex++;
                                    while (photoIndex < productionPhotosList[productionIndex].Count)
                                    {
                                        <li data-target="#index-block-@i" data-slide-to="@photoIndex"></li>
                                        photoIndex++;
                                    }
                                    photoIndex = 0;
                                }
                            }
                        </ol>
                        <!--===== CAROUSEL SLIDE -->
                        <div class="carousel-inner">
                            @{
                                <!--=== Check if photos -->
                                if (productionPhotosList[productionIndex].Count == 0)
                                {
                                    <div class="carousel-item active">
                                        <img class="d-block w-100" src="~/Content/Images/Unavailable.png" alt='@item.Title' />
                                    </div>
                                }
                                else
                                {
                                    <!--=== First slide -->
                                    <div class="carousel-item active">
                                        <img class="d-block w-100" src='@Url.Action("DisplayPhoto", "Photo", new { id = productionPhotosList[productionIndex][photoIndex].PhotoId })' alt='@productionPhotosList[productionIndex][photoIndex].Description' />
                                    </div>
                                    <!--=== More than one slide -->
                                    if (productionPhotosList[productionIndex].Count > 1)
                                    {
                                        photoIndex++;
                                        while (photoIndex < productionPhotosList[productionIndex].Count)
                                        {
                                            <div class="carousel-item">
                                                <img class="d-block w-100" src='@Url.Action("DisplayPhoto", "Photo", new { id = productionPhotosList[productionIndex][photoIndex].PhotoId })' alt='@productionPhotosList[productionIndex][photoIndex].Description' />
                                            </div>
                                            photoIndex++;
                                        }
                                        photoIndex = 0;
                                    }
                                }
                            }
                        </div>
                        <!--===== CAROUSEL CONTROLS -->
                        <a class="home-carousel-nav carousel-control-prev" href="#index-block-@i" role="button" data-slide="prev">
                            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                            <span class="sr-only">Previous</span>
                        </a>
                        <a class="home-carousel-nav carousel-control-next" href="#index-block-@i" role="button" data-slide="next">
                            <span class="carousel-control-next-icon" aria-hidden="true"></span>
                            <span class="sr-only">Next</span>
                        </a>
                    </div>

                    <!--===== PRODUCTION - Content =====-->
                    <div class="home-content">
                        <div class="home-content-inner">
                            <h2 class="home-title">@item.Title</h2>
                            <p class="home-date">@item.OpeningDay.ToString("dddd, MMMM %d, yyyy") - @item.ClosingDay.ToString("dddd, MMMM %d, yyyy")</p>
                            <a class="btn-primary" href="@Url.Action("Details", "Productions", new { id = item.ProductionId })">Details</a>
                        </div>
                    </div>
                </div>
                i++;
                productionIndex++;               
            }
        </div>
        <!--========== NAV BAR - vertical page position ==========-->
        <nav>
            <ul id="home-nav">
                @{ i = 1; }
                @foreach (var item in Model)
                {
                    <li>
                        <a class="home-nav-link" href="#index-block-@i"></a>
                    </li>
                    i++;
                }
            </ul>
        </nav>
    </main>
</div>
```

#### CSS:
```CSS
/*=========================================
========== HOMEPAGE =======================
===========================================*/

/*===== BODY - page format */
.home-body {
    position: relative;
    -webkit-box-flex: 1;
    -ms-flex: 1 0 auto;
    flex: 1 0 auto;
}

/*===== MAIN - general display, font settings */
#home-main {
    display: block;
    font-size: 100%;
    font-family: BebasNeue;
    font-weight: 400;
    line-height: 1.5;
    overflow-x: hidden;
}

/*===== HOME BLOCK - production carousels and info  */
.home-block, .home-inner {
    margin: 0;
    padding: 0;
    border: none;
    outline: 0;
    vertical-align: baseline;
    position: relative;
    overflow: hidden;
}
/*=== Carousel nav buttons */
.home-carousel-nav {
    z-index: 2;
}

/*===== TINT PHOTO OVERLAY - visable only on large screens */
.home-tint-overlay {
    position: absolute;
    top: 0px;
    left: 0px;
    right: 0px;
    bottom: 0px;
    z-index: 1;
    background: -webkit-gradient(linear,left top,left bottom,color-stop(53.92%,rgba(35,31,32,0)),color-stop(72.5%,rgba(35,31,32,.6)));
    background: linear-gradient(rgba(35,31,32,0) 53.92%,rgba(35,31,32,.6) 72.5%);
    -webkit-transition: inherit;
    transition: opacity ease-in-out 3s;
}
@media only screen and (max-width: 48em) {
    .home-tint-overlay {
        display: none;
    }
}

/*===== HOME CONTENT - Absolute position over photos, relative position for small screens */
.home-content {
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    -webkit-box-pack: center;
    -ms-flex-pack: center;
    justify-content: center;
    text-align: center;
    color: #fff;
    margin-bottom: 2.5rem;
    z-index: 1;
    width: 100%;
    max-width: 92rem;
    margin-left: auto;
    margin-right: auto;
    padding-left: 1rem;
    padding-right: 1rem;
}
/*=== Large screens- In front of Carousel */
@media only screen and (min-width: 48.0625em) {
    .home-content {
        max-width: 94rem;
        padding-left: 2rem;
        padding-right: 2rem;
    }
}
/*=== X-Large screens - In front of Carousel */
@media only screen and (min-width: 64.125em) {
    .home-content {
        max-width: 99.75rem;
        padding-left: 4.875rem;
        padding-right: 4.875rem;
    }
}
/*=== Small screens - drops below Carousel on page */
@media (max-width: 48em) {
    .home-content {
        position: relative;
        text-align: left;
        -webkit-box-pack: start;
        -ms-flex-pack: start;
        justify-content: flex-start;
        margin: .5rem 0 1rem;
        z-index: 0;
        opacity: 1;
    }
}

.home-content-inner {
    width: 100%;
}

/*===== PRODUCTION TITLE */
.home-title {
    display: block;
    margin: 0;
    line-height: 1;
    font-size: 13rem;
    font-weight: 400;
    background: transparent;
    color: var(--light-color);
    text-shadow: var(--dark-color) 0px 0px 10px;    
}
/*=== X-Large screens */
@media only screen and (max-width: 80em) {
    .home-title {
        font-size: 10rem;
    }
}
/*=== Large screens */
@media only screen and (max-width: 64.0625em) {
    .home-title {
        font-size: 7.5rem;
    }
}
/*=== Small screens */
@media only screen and (max-width: 48em) {
    .home-title {
        font-size: 2.5rem;
        color: var(--light-color);
        margin-bottom: 0 !important;
    }
}

/*===== PRODUCTION DATE */
.home-date {
    font-family: Arial, sans-serif;
    font-size: 1.5rem;
    margin: -.25rem 0 1.25rem;
}
/*=== Small screens */
@media only screen and (max-width: 48em) {
    .home-date {
        color: var(--light-color);
        font-size: .875rem;
        margin: 0 0 .25rem;
    }
}

/*===== PRODUCTION DETAILS BUTTON */
.home-content .btn-primary {
    display: inline-block;
    font-weight: 700;
    margin-bottom: 1rem;
    padding: .75rem 1rem;
    font-family: Arial;
    text-decoration: none;
    text-transform: uppercase;
    text-align: center;
    min-width: 7.5rem;
    border-radius: 0;
    font-size: 1rem;
    line-height: 1.5;
    background-color: var(--main-bg-color);
    color: var(--light-color);
    border: 1px solid var(--main-bg-color);
}
/*=== Button focus, hover, active */
.home-content .btn-primary:focus {
    background-color: rgba(0, 0, 0, .6);
    color: var(--main-bg-color);
    border: 1px solid var(--main-bg-color);
    box-shadow: none;
}
.home-content .btn-primary:active:hover {
    background-color: rgba(0, 0, 0, .6);
    color: var(--main-bg-color);
    border: 1px solid var(--main-bg-color);
    box-shadow: none !important;
}
.home-content .btn-primary:hover {
    background-color: rgba(219, 26, 17, .7);
}
/*=== Small screens */
@media only screen and (max-width: 48em) {
    .home-content .btn-primary {
        margin-top: .625rem;
        font-size: .875rem;
        line-height: 1.7143;
        color: var(--dark-color);
    }
}

/*========== HOME NAV - dots on right side of screen, display current vertical position and jump to vertical position  */
/*===== Positioning of navbar on screen */
#home-nav {
    position: fixed;
    top: 50%;
    right: 2.5rem;
    color: #fff;
    z-index: 100;
    list-style-type: none;
    padding: 0;
    margin: 0;
}
/*=== Not visable on small screens */
@media only screen and (max-width:48em) {
    #home-nav {
        display: none;
    }
}

/*===== Dot format */
#home-nav a {
    display: block;
    border-bottom: none;
    margin-bottom: .5rem;
    background-color: #fff;
    width: .75rem;
    height: .75rem;
    border: 1px solid var(--dark-color);
    border-radius: 50%;
    cursor: pointer;
}

/*===== Dot hover, active, focus */
#home-nav a:hover {
    background-color: var(--secondary-color);
}
#home-nav li.active a {
    background-color: var(--main-bg-color);
}
#home-nav a:focus {
    box-shadow: 0px 2px 0 0 var(--dark-color) !important;
    outline: 0;
}
```

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

#### Before/After:
![alt text](https://github.com/alex-moffat/C-Sharp-Large-Projects/blob/master/CS_Story_3.jpg "Story_3")

#### CS
```CS
public ActionResult Create([Bind(Include = "PartID,Production,Person,Character,Type,Details")] Part part)
    {
        //=== VALIDATION - PRODUCTION check if selected from dropdown
        if (Request.Form["Production"] != "")
        {
            int productionID = Convert.ToInt32(Request.Form["Production"]);
            var production = db.Productions.Find(productionID);
            part.Production = production;
            ModelState.Remove("Production"); // manual remove error - throws by default due to Part model [Required] validation can't match to dropdown
        }
        //=== VALIDATION - PERSON check if selected from dropdown
        if (Request.Form["Person"] != "")
        {
            int castID = Convert.ToInt32(Request.Form["Person"]);
            var person = db.CastMembers.Find(castID);
            part.Person = person;
            ModelState.Remove("Person"); // manual remove error - throws by default due to Part model [Required] validation can't match to dropdown
        }
        //=== VALIDATION - FORM
        if (ModelState.IsValid)
        {
            //=== IS VALID - lookup production and cast member objects based on form value "Id" for each
            db.Parts.Add(part);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            //=== NOT VALID - reload dropdown menus then return part to view 
            ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName");
            ViewData["CastMembers"] = new SelectList(db.CastMembers.ToList(), "CastMemberId", "Name");
            ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");
            return View(part);
        }            
    }
```


### Story 4: Enable photo update during Production creation  
-	Create an input field in the Productions Create page that allows the admin to be able to select a photo from their file system.
-	When the Create form submits, new Photo and Production Photo records are created
-   The Production's DefaultPhoto field is set to new ProductionPhoto Id.
-	File input placed below the Closing Day input field.
-	<b>Create validation method to check if uploaded file is a photo file type</b>
-	Make name field required for production creation
-	Name the new photo in Photos DB the production name and make details of production photo “Default Photo”
-	Add photo validation feedback to user that upload file is not a valid photo and allow new selection before create.
-	Allow Create production without photo if no upload file is given
-	Use selected Season after failed validation and current season on new page load 

### Story 5: Add custom validation and fix display bug
-	Prevent the creation of Productions with null Evening and Matinee showtimes.  
-	Fix error on the Details page of Production will not display a production with null showtimes.  
-	Validate that at least one showtime is provided to allow saving a new production.
-	Create validation messages "At least one showtime must be specified" on the Productions Create page if the user attempts to create a Production where Evening and Matinee showtimes are both null.
-	Ensure that users can navigate to the Productions Details page without throwing an error if there are null Matinee or Evening showtimes.
-	Display “Not Playing” on the Productions details page for null Evening and Matinee showtimes. 


