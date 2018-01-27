# Team-Alpha-2018

AcuFace:

The approach here is to demonstrate how simple it is to integrate Acumatica using its customization framework with the large and growing specialized web services to create new solutions.
Microsoft Azure and AWS platforms have a large amount of services that can be easily integrated using the same design patterns as what we outline below.

This project uses Microsoft’s Cognitive Services Face API 1.0.

Here is what will need to be done
1.  Will need to customize Security Preferences and add a two new fields, Subscription id and service orl. (SM201060)
2.  Under user security manage add a new form to define CS (Cognitive services) Groups. You can have one or more groups.
a.  enter a Name and description for the group
b.  Add users to the group
c.  Add a button to send to CS web service.
3.  Add a CS Face form. In this form need to be able to upload or take a user picture to send to CS server.
a.  Pick group and user from the group
b.  Have a standard Acumatica picture box with a button for add picture
c.  Save that sends picture and user/group to CS service
4.  Override the approve button on the code on the approval form to prompt for a picture and get validation of facial recognition from CS

AcuChat:

This project will add a skype like icon to the app. We were looking at this as a way to send real time messages in our system. Also can be applied to Support Chat.

1.  Click the icon and a dialog opens.
a.  Recipient selector would show logged on users.
b.  Pick a person
c.  Type a message
d.  Send
2.  The recipient would get the message via standard browser notifications using HTML 5


