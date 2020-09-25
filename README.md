# C-Sharp-Large-Projects

## CONTENT
- [LIVE PROJECT](#live-project)
- [BLACKJACK DEMO - Console Application](#blackjack-demo)

## LIVE PROJECT
I had an opportunity to work on a two-week spring of a large-scale ASP .NET MVC website with 6 other developers. Within a couple days I was able to get up speed on a large existing codebase from multiple prior contributors that was being altered to satisfy future roadmap specifications. During my time on the project I seamlessly merged code that made significant improvements to a large existing web application without comprising legacy functionality. During the two-week project I completed five stories that improved back-end functionality and front-end design while identifying and fixing existing bugs. Even during the short span of time, I was able to connect and collaborate with team members to help accelerate their stories.

### Description
This project is built using ASP .Net MVC and Entity Framework. This project is the interactive website for managing the content and productions for a theater/acting company. It's meant to be a content management service (aka CMS) for users who are not technically saavy and want to easily manage what displays in their website. It's also meant to help manage login capability for subscribers, and maintain a wiki of past performances and performers. 

## STORIES
- [Story 1: Align Checkboxes](#story-1-align-checkboxes)
- [Story 2: Replace static images on home page with carousels](#story-2-replace-static-images-on-home-page-with-carousels)
- [Story 3: Emulate part details card with part create form](#story-3-emulate-part-details-card-with-part-create-form)
- [Story 4: Enable photo update during Production creation](#story-4-enable-photo-update-during-production-creation)
- [Story 5: Add custom validation and fix display bug](#story-5-add-custom-validation-and-fix-display-bug)


## STORY DETAILS
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

#### CSHTML:
```CSHTML

@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()
    
    <div class="mt-4 container text-center">
        <!--===== TYPE =====-->
        <div class="form-row align-items-center justify-content-center form-group">
            <div>
                <h4>Creating a new</h4>
            </div>
            <div class="mx-2">
                @Html.EnumDropDownListFor(model => model.Type, htmlAttributes: new { @class = "form-control", @onchange = "GetType()" })
                <!--===== IF anything except Actor type is selected, the Character field will be read only, hidden from view, and label changed to type and italic removed -->
                <script type="text/javascript">
                    function GetType() {
                        var selectedType = $('#Type option:selected').text();
                        //=== ACTOR
                        if (selectedType !== "Actor") {
                            $('#Character').attr('readonly', 'true')
                            $('#Character').val($('#Person option:selected').text());
                            $('#Character').addClass('hide');                            
                            $('#Character-Label').html(selectedType);
                            $('#Character-Label').removeClass('font-italic');
                            $('#Character-Label').addClass('font-weight-bold');
                            $('#Character-Label').addClass('mb-4');
                        }
                        //=== NON-ACTOR
                        else
                        {
                            $('#Character').removeAttr('readonly')
                            $('#Character').val("");
                            $('#Character').removeClass('hide');
                            $('#Character-Label').html('Playing');
                            $('#Character-Label').addClass('font-italic');
                            $('#Character-Label').removeClass('font-weight-bold');
                            $('#Character-Label').removeClass('mb-4');
                        }
                    }
                    $(document).ready(GetType);
                </script>
                @Html.ValidationMessageFor(model => model.Type, "", new { @class = "text-danger" })
            </div>
            <div>
                <h4>Part</h4>
            </div>            
        </div>
        
        <!--========== CARD ==========-->
        <div class="d-flex justify-content-center">
            <div class="card part-create border-white">
                <div class="card-body">
                    @Html.ValidationSummary(true, "", new { @class = "text-white" })
                    <!--===== PERSON - cast member =====-->
                    <div class="form-group mb-0">
                        @Html.DropDownList("Person", (IEnumerable<SelectListItem>)ViewData["CastMembers"], "Cast Member", htmlAttributes: new { @class = "form-control part-create pl-4", @onchange = "GetPerson()" })
                        <!--=== takes the person name and populates it in character field if it is not Actor -->
                        <script type="text/javascript">
                            function GetPerson() {
                                var selectedmember = $('#Person option:selected').text();
                                var selectedType = $('#Type option:selected').text();
                                if (selectedType !== "Actor")
                                    document.getElementById("Character").value = selectedmember;                                    
                            }
                            $(document).ready(GetPerson);
                        </script>
                        @Html.ValidationMessageFor(model => model.Person, "", new { @class = "text-white" })
                    </div>
                    <hr class="mt-0 mb-5 bg-white" style="height:2px;">
                    <!--===== CHARACTER - Use static label "Playing" when actor or type when not, character field hidden when not actor =====-->
                    <div class="form-group mb-2">
                        @Html.LabelFor(model => model.Character, "Playing", htmlAttributes: new { @id= "Character-Label", @class = "control-label font-italic mb-0" })
                        @Html.EditorFor(model => model.Character, new { htmlAttributes = new { @class = "form-control part-create text-center", @placeholder = "Character" } })
                        @Html.ValidationMessageFor(model => model.Character, "", new { @class = "text-white" })
                    </div>
                    <!--===== PRODUCTION - Use static label "In the Production" =====-->
                    <div class="form-group">
                        @Html.LabelFor(model => model.Production, "In the Production", htmlAttributes: new { @class = "control-label font-italic mb-0" })
                        @Html.DropDownList("Production", (IEnumerable<SelectListItem>)ViewData["Productions"], "Production", htmlAttributes: new { @class = "form-control part-create" })
                        @Html.ValidationMessageFor(model => model.Production, "", new { @class = "text-white" })
                    </div>
                    <!--===== DETAILS =====-->
                    <div class="form-group mt-5">
                        @Html.LabelFor(model => model.Details, htmlAttributes: new { @class = "control-label font-weight-bold mb-2" })
                        @Html.TextAreaFor(model => model.Details, new { @class = "form-control part-create", @rows = 6, @placeholder = "Details..." })
                        @Html.ValidationMessageFor(model => model.Details, "", new { @class = "text-white" })
                    </div>

                </div>
            </div>
        </div>
        <!--===== SUBMIT =====-->
        <div class="mt-4 form-row align-items-center justify-content-center form-group">            
            <button type="submit" value="Create" class="part-create btn btn-success btn-lg">Create</button>            
        </div>       
    </div>
}
```

#### CSS:
```CSS
/*========== PART CREATE ==========*/

/*===== CARD - add part create style to Bootstrap base card */
.card.part-create {
    background-color: var(--main-bg-color);
    border-radius: 25px;
    outline-color: white;
    min-width: 300px;
    max-width: 550px;    
}

/*===== FORM CONTROL - add part create style to Bootstrap base */
.form-control.part-create {
    background-color: var(--main-secondary-color);
    color: white;    
}
/*=== center align the value selected in the dropdown list */
select.form-control.part-create {
    text-align-last: center; 
}
/*=== hide element selectively from jQuery add/remove class method */
.form-control.part-create.hide {
    display: none;
}

/*===== DROPDOWN Placeholder option */
select.form-control.part-create option[value=""] {
    color: rgba(255,255,255,.4);
}

/*=== INPUT Placeholder color */
.form-control.part-create::-webkit-input-placeholder {
    color: rgba(255,255,255,.4);
}
.form-control.part-create::placeholder {
    color: rgba(255,255,255,.4);
}
.form-control.part-create:-ms-input-placeholder {
    color: rgba(255,255,255,.4);
}
.form-control.part-create:-moz-placeholder {
    color: rgba(255,255,255,.4);
}
.form-control.part-create::-moz-placeholder {
    color: rgba(255,255,255,.4);
}

/*===== BUTTON - Submit under card */
button.part-create {
    max-width: 500px;
    width: 90%;
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

#### CS:
```CS
public ActionResult Create([Bind(Include = "ProductionId,Title,Playwright,Description,Runtime,OpeningDay,ClosingDay,DefaultPhoto,ShowtimeEve,ShowtimeMat,TicketLink,Season,IsCurrent,IsWorldPremiere")] Production production, HttpPostedFileBase uploadFile)
{
    //========== VALIDATION
    //===== PHOTO - Check if photo is not null but not a valid photo format
    if (uploadFile != null && !PhotoController.ValidatePhoto(uploadFile))
    {
        ModelState.AddModelError("DefaultPhoto", "File must be a valid photo format.");
        ViewData["upload_file"] = uploadFile;
    }
    if (ModelState.IsValid)
    {
        //========== DEFAULT PHOTO ==========
        //--- save photo using photo controller, save entry to ProductionPhotos, photoName set to production title, description set to "Default Photo"
        if (uploadFile != null)
        {
            //----- Save New Photo or retrieve existing photo if the same - using photo controller
            Debug.WriteLine("Saving photo...");
            int photoId = PhotoController.CreatePhoto(uploadFile, production.Title);
            //----- Save New ProductionPhoto
            var productionPhoto = new ProductionPhotos() { PhotoId = photoId, Title = production.Title, Description = "Default Photo" };
            db.ProductionPhotos.Add(productionPhoto);
            db.SaveChanges();
            //----- Save New Production, add DefaultPhoto object reference to production
            production.DefaultPhoto = productionPhoto;
            db.Productions.Add(production);
            db.SaveChanges();
            //----- Add Production object reference to productionPhoto
            productionPhoto.Production = production;
            db.Entry(productionPhoto).State = EntityState.Modified;
            db.SaveChanges();
        }
        //========== NO PHOTO ==========
        else
        {
            db.Productions.Add(production);
            db.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    return View(production);
}

//========== VALIDATE PHOTO - Test if input is photo, return True if a photo format
public static bool ValidatePhoto(HttpPostedFileBase postedFile)
{
    Debug.WriteLine("Validating photo...");
    const int ImageMinimumBytes = 512;
    //-------------------------------------------
    //  Check the image mime types
    //-------------------------------------------
    if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(postedFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(postedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(postedFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    //-------------------------------------------
    //  Check the image extension
    //-------------------------------------------
    var postedFileExtension = Path.GetExtension(postedFile.FileName);
    if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    //-------------------------------------------
    //  Attempt to read the file and check the first bytes
    //-------------------------------------------
    try
    {
        if (!postedFile.InputStream.CanRead)
        {
            return false;
        }
        //----- Check whether the image size below the lower limit or not
        if (postedFile.ContentLength < ImageMinimumBytes)
        {
            return false;
        }
        //----- Read the file
        byte[] buffer = new byte[ImageMinimumBytes];
        postedFile.InputStream.Read(buffer, 0, ImageMinimumBytes);
        string content = System.Text.Encoding.UTF8.GetString(buffer);
        if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
        {
            return false;
        }
    }
    catch (Exception)
    {
        return false;
    }
    //-------------------------------------------
    //  Try to instantiate new Bitmap to see if .NET will throw exception
    //-------------------------------------------
    try
    {
        using (var bitmap = new Bitmap(postedFile.InputStream))
        {
        }
    }
    catch (Exception)
    {
        return false;
    }
    finally
    {
        postedFile.InputStream.Position = 0;
    }
    return true;
}
```

#### CSHTML:
```CSHTML
<!--===== DEFAULT PHOTO =====-->
<div class="form-group">
    @Html.LabelFor(model => model.DefaultPhoto, htmlAttributes: new { @class = "control-label col-md-4 inputLabel" })
    <div class="col-md-10 formBox text-center">
        <!--=== Manual validation of the photo upload handled in ProductionsController -->
        @Html.TextBox("uploadFile", null, new { type = "file", @class = "text-black" })
        <!--=== Hidden required to validate DefaultPhoto (foreignkey reference) that is a non-file datatype -->
        @Html.HiddenFor(m => m.DefaultPhoto)
        <!--=== Puts validation message on new line -->
        <div>@Html.ValidationMessageFor(model => model.DefaultPhoto, "", new { @class = "text-white" })</div>
    </div>
</div>

<!--===== SEASON =====-->
<div class="form-group">
    @Html.LabelFor(model => model.Season, htmlAttributes: new { @class = "control-label col-md-4 inputLabel" })
    <div class="col-md-10 formBox">
        <!--=== Check if return after failed validation (use form data filled in), else new Create view will use currentSeason as default===-->
        @if (Model != null)
        {
            @Html.EditorFor(model => model.Season, new { htmlAttributes = new { @class = "form-control" } })
        }
        else
        {
            @Html.EditorFor(model => model.Season, new { htmlAttributes = new { @class = "form-control", @Value = (int)ViewData["current_season"] } })
        }
        @Html.ValidationMessageFor(model => model.Season, "", new { @class = "text-danger" })
    </div>
</div>
```

### Story 5: Add custom validation and fix display bug
-	Prevent the creation of Productions with null Evening and Matinee showtimes.  
-	Fix error on the Details page of Production will not display a production with null showtimes.  
-	Validate that at least one showtime is provided to allow saving a new production.
-	Create validation messages "At least one showtime must be specified" on the Productions Create page if the user attempts to create a Production where Evening and Matinee showtimes are both null.
-	Ensure that users can navigate to the Productions Details page without throwing an error if there are null Matinee or Evening showtimes.
-	Display “Not Playing” on the Productions details page for null Evening and Matinee showtimes. 

#### CS:
```CS
public ActionResult Create([Bind(Include = "ProductionId,Title,Playwright,Description,Runtime,OpeningDay,ClosingDay,DefaultPhoto,ShowtimeEve,ShowtimeMat,TicketLink,Season,IsCurrent,IsWorldPremiere")] Production production, HttpPostedFileBase uploadFile)
{
    //========== VALIDATION ==========
    //===== PHOTO - Check if photo is not null but not a valid photo format
    if (uploadFile != null && !PhotoController.ValidatePhoto(uploadFile))
    {
        ModelState.AddModelError("DefaultPhoto", "File must be a valid photo format.");                
    }
    //===== SHOWTIME - at list one (ShowtimeEve, ShowtimeMat) need to be assigned 
    if (production.ShowtimeEve == null && production.ShowtimeMat == null)
    {
        ModelState.AddModelError("ShowtimeEve", "At least one showtime must be specified.");
        ModelState.AddModelError("ShowtimeMat", "At least one showtime must be specified.");
    }
    //========== SAVE ==========
    if (ModelState.IsValid)
    {
        //===== DEFAULT PHOTO 
        //--- save photo using photo controller, save entry to ProductionPhotos, photoName set to production title, description set to "Default Photo"
        if (uploadFile != null)
        {
            //----- Save New Photo or retrieve existing photo if the same - using photo controller
            Debug.WriteLine("Saving photo...");
            int photoId = PhotoController.CreatePhoto(uploadFile, production.Title);
            //----- Save New ProductionPhoto
            var productionPhoto = new ProductionPhotos() { PhotoId = photoId, Title = production.Title, Description = "Default Photo" };
            db.ProductionPhotos.Add(productionPhoto);
            db.SaveChanges();
            //----- Save New Production, add DefaultPhoto object reference to production
            production.DefaultPhoto = productionPhoto;
            db.Productions.Add(production);
            db.SaveChanges();
            //----- Add Production object reference to productionPhoto
            productionPhoto.Production = production;
            db.Entry(productionPhoto).State = EntityState.Modified;
            db.SaveChanges();
        }
        //===== NO PHOTO 
        else
        {
            db.Productions.Add(production);
            db.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    return View(production);
}
```

CSHTML:
```CSHTML
<!--===== SHOWTIMES - Display only if available-->
<li class="list-group-item">
  Evening Showtime
    <dl>
        @{
            if (Model.ShowtimeEve.HasValue)
            {
                <dd class="col">@Model.ShowtimeEve.Value.ToString("h:mm tt")</dd>
            }
            else
            {
                <dd class="col">Not Playing</dd>
            }
        }
    </dl>
</li>
<li class="list-group-item">
    Matinee Showtime
    <dl>
        @{
            if (Model.ShowtimeMat.HasValue)
            {
                <dd class="col">@Model.ShowtimeMat.Value.ToString("h:mm tt")</dd>
            }
            else
            {
                <dd class="col">Not Playing</dd>
            }
        }                    
    </dl>
</li>
```

## BLACKJACK DEMO
I developed a console application to demonstrate OOP programming and logic with C#. This fully functioning demo allows multiple players to play a casino style blackjack rules with a virtual dealer. Multiple decks are created and scaled with the number of players. Player and dealer chip accounting and balances are maintained, betting limits and user errors are accounted for. Casino rules twenty-one logic is used to establish winners and losers. Players can leave or continue and are automatically removed from game when balance reaches zero. Sample code snippets provided below. Fraud logging to database for those trying to bet negative values (intentional) and admin login to display all DB records to demonstrate DB handling. The main program was written in the Blackjack namespace with reference to the Casino namespace.   

### Select Code Snippets
- [Main Blackjack program](#program)
- [Blackjack game logic](#blackjackgame)

#### Program
```CS
namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            //========== CASINO NAME
            const string casinoName = "Grand Hotel and Casino";

            //========== LOG FILE
            string logDir = Directory.GetCurrentDirectory() + @"\logs";
            string logFile = logDir + @"\log.txt";
            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
            string logTxt = string.Format("========== BLACKJACK LOG ==========\n{0}\n", DateTime.Now);
            File.WriteAllText(logFile, logTxt);

            //========== GAME SETUP
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Clear();
            Console.WriteLine("\n===================================================\n=====( Welcome to the {0} )=====\n===================================================\n", casinoName);
            Game game = new BlackjackGame();

            //========== ADD PLAYERS
            const int maxPlayers = 7;
            bool addPlayers = true;
            string playerName;            

            while (addPlayers && game.Players.Count < maxPlayers)
            {
                Console.WriteLine("\nPlayer, what is your name?");
                playerName = Console.ReadLine();
                playerName = playerName[0].ToString().ToUpper() + playerName.Substring(1);
                //===== ADMIN ENTRY
                if (playerName == "Admin")
                {
                    List<ExceptionEntity> exceptions = ReadExceptions();
                    foreach (var e in exceptions)
                    {
                        Console.Write(e.Id + " | ");
                        Console.Write(string.Format("{0} | ", e.ExceptionType));
                        Console.Write(string.Format("{0} | ", e.ExceptionMessage));
                        Console.WriteLine(string.Format("{0} \n", e.TimeStamp));                        
                    }
                    Console.Read();
                    return;
                }
                //===== PLAYER ENTRY
                Player player = new Player(playerName); // asks for player bank if not provided
                if (player.Balance > 0)
                {
                    Console.WriteLine("Hello {0}. Would you like to join a game of Blackjack right now?", playerName);
                    if (Console.ReadLine().ToLower().Contains("y"))
                    {
                        game += player;
                        Console.WriteLine("-->{0} added to game.", playerName);
                    }                    
                }
                if (game.Players.Count < maxPlayers)
                {
                    Console.WriteLine("\nIs someone else joining today?");
                    if (Console.ReadLine().ToLower().Contains("n")) addPlayers = false;
                }
            }
            
            //========== PLAY - continue to play rounds until NO players are actively playing or have a balance > 0
            while (game.Players.Count > 0)
            {
                //----- Play game
                try
                {
                    game.Play();
                }
                catch (FraudException e)
                {
                    UpdateDbException(e);
                    Console.WriteLine("SECURITY! Throw this person out.");
                    Console.ReadLine();
                    return;
                }
                catch (Exception e)
                {
                    UpdateDbException(e);
                    Console.WriteLine("ERROR. Please contact your system administrator.");
                    Console.WriteLine("ERROR: " + e.Message);
                    Console.ReadLine();
                    return;
                }
                //----- Player removal
                List<Player> removals = new List<Player>();
                removals = game.Players.Where(x => !x.ActivelyPlaying || x.Balance == 0).ToList();
                foreach (Player player in removals)
                {
                    game -= player;
                }                
            }
            
            //========== GAME OVER
            Console.WriteLine("\n===Thank you for playing.");
            Console.WriteLine("Feel free to look aroung the casino. Bye for now.");


            //========== TESTS ==========

            //string str1 = "Here is some text.\nMore on a new line.\tHere is some after a tab.";
            //File.WriteAllText(logFile, str1);

            //string txt = File.ReadAllText(logFile);
            //Console.WriteLine(txt);


            //===== CREATE DECK
            //Deck deck = new Deck();

            //===== SHUFFLE DECK
            //deck.Shuffle(times: 4, true);

            //===== lambda functions
            //int count = deck.Cards.Count(x => x.Face == Face.Ace); 
            //Console.WriteLine(count);

            //List<Card> newList = deck.Cards.Where(x => x.Face == Face.Ace).ToList();
            //foreach (Card card in newList) { Console.WriteLine(card.Face); }

            //List<int> numList = new List<int>() { 1,2,3,535,342,23 };
            //int sum = numList.Sum();
            //Console.WriteLine(sum);
            //int sumPlus = numList.Sum(x => x + 1);
            //Console.WriteLine(sumPlus);
            //int max = numList.Max();
            //Console.WriteLine(max);
            //int min = numList.Min();
            //Console.WriteLine(min);
            //int sumBig = numList.Where(x => x > 100).Sum();
            //Console.WriteLine(sumBig);
            //Console.WriteLine(numList.Where(x => x > 100).Sum());

            //===== Overloaded operator
            //Game game = new BlackjackGame() { Name = "BlackJack", Dealer = "Doc Holliday", Players = new List<Player>() };
            //Player p1 = new Player() { Name = "Wyatt Earp" };
            //Player p2 = new Player() { Name = "Jesse James" };
            //game = game + p1 + p2;
            //game.ListPlayers();
            //game = game - p2;
            //game.ListPlayers();

            //deck.ListCards(loop: "foreach");

            //Dealer dealer = new Dealer();
            //dealer.Name = "Alex";
            //Console.WriteLine(dealer.Name); // can get/set public properties when base class is not explicitly public

            //Game game2 = new Game(); // can't create a new object based on an abstract class

            //===== HOLD OPEN - till enter is pressed
            Console.ReadLine();
        }

        private static void UpdateDbException(Exception e)
        {
            string connectionStr = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = BalckjackGame; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            string queryStr = @"INSERT INTO Exceptions (ExceptionType, ExceptionMessage, TimeStamp) VALUES
                                (@ExceptionType, @ExceptionMessage, @TimeStamp)";
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(queryStr, connection);
                command.Parameters.Add("@ExceptionType", SqlDbType.VarChar);
                command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar);
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime);
                command.Parameters["@ExceptionType"].Value = e.GetType().ToString();
                command.Parameters["@ExceptionMessage"].Value = e.Message;
                command.Parameters["@TimeStamp"].Value = DateTime.Now;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        private static List<ExceptionEntity> ReadExceptions()
        {
            string connectionStr = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = BalckjackGame; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            string queryStr = @"SELECT Id, ExceptionType, ExceptionMessage, TimeStamp FROM Exceptions";
            List<ExceptionEntity> exceptions = new List<ExceptionEntity>();
            
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(queryStr, connection);
                
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ExceptionEntity e = new ExceptionEntity(Convert.ToInt32(reader["Id"]), reader["ExceptionType"].ToString().Trim(), reader["ExceptionMessage"].ToString().Trim(), Convert.ToDateTime(reader["TimeStamp"]));
                    exceptions.Add(e);
                }
                connection.Close();
            }
            return exceptions;
        }
    }
}
```

BlackjackGame
```CS
namespace Casino.Blackjack
{
    public class BlackjackGame : Game, IWalkAway
    {
        //===== CONSTRUCTOR
        public BlackjackGame() : base()
        {
            // using constructor from base Class "Game"
            // creates new instances of Players, Bets
            Dealer = new BlackjackDealer("", 0);
        }

        //===== PROPERTIES
        public BlackjackDealer Dealer { get; set; }

        //===== PLAY
        public override void Play() //override satisfies the requirement for this child to have the parent method
        {
            //----- START set Dealer and declare variables at the beginning of game 
            if (Dealer.Name == "") 
            {
                Dealer = new BlackjackDealer(name: "Doc Holliday", bank: int.MaxValue);
                ListPlayers();
            }
            int bet;
            bool isValid, winner;

            //----- RESET each round
            Dealer.Stay = Dealer.Blackjack = winner = false;
            Dealer.Hand = new List<Card>();
            Dealer.Deck = new Deck(deckCount:Players.Count);
            foreach (Player p in Players)
            {
                p.Hand = new List<Card>();
                p.Stay = false;
            }

            //----- BET for each player
            Console.WriteLine("\n=== Place Bets...");
            foreach (Player p in Players)
            {
                isValid = false;
                while (!isValid)
                {
                    try
                    {
                        Console.WriteLine("{0} your bet:", p.Name);
                        bet = Convert.ToInt32(Console.ReadLine());
                        if (p.Bet(bet)) 
                        { 
                            isValid = true;
                            Bets[p] = bet;
                        }                        
                    }
                    catch (FraudException)
                    {
                        p.ActivelyPlaying = false;
                        throw new FraudException(string.Format("Security! {0} has tried to commit fraud",p.Name));
                        // Console.WriteLine("SECURITY! Throw this person out.");
                        // Console.ReadLine();
                        // return;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Oops...something went wrong:");
                        Console.WriteLine(e.Message);
                    }
                }   
            }

            //----- DEAL START
            Console.WriteLine("\n=== Dealing...");
            for (int i = 0; i < 2; i++)
            {
                Dealer.Deal(Dealer.Hand, Dealer.Name);
                foreach (Player p in Players) Dealer.Deal(p.Hand, p.Name);                
            }
            
            //----- CHECK BLACKJACK - If dealer has Blackjack all players lose except those that also have Blackjack (Draw)
            Dealer.Blackjack = BlackjackRules.CheckBlackjack(Dealer.Hand);
            if (Dealer.Blackjack)
            {
                //--- Dealer Blackjack, all payouts, return
                Console.WriteLine("\n=== Dealer gets Blackjack!");
                foreach (Player p in Players)
                {
                    if (BlackjackRules.CheckBlackjack(p.Hand))
                    {
                        //--- DRAW player blackjack
                        Dealer.Payout(Dealer, p, Bets, condition:"draw");                                                
                    }
                    else
                    {
                        //--- LOSE no blackjack
                        Dealer.Payout(Dealer, p, Bets, condition: "lose");
                    }                    
                }
                return;
            }
            else
            {
                //--- Dealer no Blackjack, check player Blackjack, payout if winner
                winner = Players.Any(p => BlackjackRules.CheckBlackjack(p.Hand));
                if (winner)
                {
                    foreach (Player p in Players)
                    {
                        if (BlackjackRules.CheckBlackjack(p.Hand))
                        {
                            Dealer.Payout(Dealer, p, Bets, condition: "blackjack");
                        }                        
                    }                    
                }
            }

            //----- CONTINUE PLAY
            Console.WriteLine("\n=== Continue Play...");
            foreach (Player p in Players)
            {
                while (!p.Stay)
                {
                    //--- Hand display
                    Console.WriteLine("\nDealer hand:");
                    foreach (Card c in Dealer.Hand) Console.WriteLine("- {0}", c.ToString());
                    Console.WriteLine("\n{0} your cards are:", p.Name);
                    foreach (Card c in p.Hand) Console.WriteLine("- {0}", c.ToString());
                    
                    //--- Hit or Stay
                    Console.WriteLine("\n Hit or Stay?");
                    if (Console.ReadLine().ToLower().Contains("s")) 
                    {
                        p.Stay = true;
                        break;
                    }
                    else
                    {
                        Dealer.Deal(p.Hand, p.Name);
                    }
                    
                    //--- Check Busted
                    if (BlackjackRules.CheckBusted(p.Hand)) Dealer.Payout(Dealer, p, Bets, condition: "bust");                    
                }
            }

            //----- DEALER PLAY
            if (Bets.Count() == 0) return; // check if all players are out
            Dealer.Stay = BlackjackRules.CheckDealerStay(Dealer.Hand);
            while (!Dealer.Stay && !Dealer.Bust)
            {
                Console.WriteLine("\n=== Dealer is hitting...");
                Dealer.Deal(Dealer.Hand, Dealer.Name);
                Dealer.Bust = BlackjackRules.CheckBusted(Dealer.Hand);
                Dealer.Stay = BlackjackRules.CheckDealerStay(Dealer.Hand);
            }
            
            //----- DEALER BUST
            if (Dealer.Bust)
            {
                Console.WriteLine("\n=== Dealer Busted!");
                //--- PAYOUT function - pay only thoses players that still have bets
                foreach (Player p in Players.Where(x=> Bets.ContainsKey(x)))
                {
                    Dealer.Payout(Dealer, p, Bets, condition: "win");
                }
                
                ////--- ALTERNATIVE - Lambda function method
                //foreach (KeyValuePair<Player, int> entry in Bets)
                //{    
                //    Players.Where(p => p == entry.Key).First().Balance += (entry.Value * 2);
                //}                
            }
            //----- DEALER STAY - check only thoses players that still have bets
            else if (Dealer.Stay)
            {
                Console.WriteLine("\n=== Dealer is staying.");
                foreach (Player p in Players.Where(x => Bets.ContainsKey(x))) 
                {
                    switch (BlackjackRules.CheckWin(Dealer.Hand, p.Hand))
                    {
                        case true: //--- player win
                            Dealer.Payout(Dealer, p, Bets, condition: "win");
                            break;
                        case false: //--- dealer win
                            Dealer.Payout(Dealer, p, Bets, condition: "lose");
                            break;
                        default: //--- draw
                            Dealer.Payout(Dealer, p, Bets, condition: "draw");
                            break;                        
                    }
                }                
            }
            return;
        }

        //===== LIST PLAYERS
        public override void ListPlayers()
        {
            Console.WriteLine("===== Welcome To BLACKJACK =====");
            Console.WriteLine("Your dealer is {0}.", Dealer.Name);
            base.ListPlayers(); // this line is the same as all the code within base class method
        }
        //===== WALK AWAY
        public void WalkAway(Player player)
        {
            throw new NotImplementedException();
        }   
        
    }
}
```
