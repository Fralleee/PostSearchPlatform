# Post Search Solution

This project is a dynamic web application built on ASP.NET Core MVC, primarily developed for learning purposes. Serving as an educational tool, it offers a practical experience in building a full-featured blogging platform with modern web technologies.

---

## Features

The application includes key features like:

1. **Creating new posts**: Users can create new blog posts that are stored and displayed on the website.
2. **Searching posts**: Users can perform real-time search for blog posts via the search bar. The search functionality uses a JavaScript fetch API to asynchronously retrieve matching posts from the server.
3. **Viewing posts**: Users can view a list of posts, each displaying key details such as the title, created date, and a brief description. Each post includes a thumbnail image and a link to the post's detail view.
4. **Post Pagination**: The homepage shows a maximum of 20 posts at a time to manage the display of content and maintain page performance.
5. **Reindexing posts**: There's a function to reindex posts, possibly indicating some sort of search optimization or database management functionality.

## Technologies used

This project harnesses the power of a variety of cutting-edge web technologies, each contributing unique features and capabilities:

- **.NET v6.0 Core**: As the backbone of the application, .NET Core offers a robust and high-performance framework for building dynamic web applications. It supports cross-platform development and enables a clean, maintainable codebase.
- **Dependency Injection**: This design pattern is integral to the structure of the application, facilitating loose coupling and enhancing testability. It simplifies object management and promotes cleaner, more reusable code.
- **ElasticSearch**: An open-source search engine, ElasticSearch allows efficient full-text search and real-time indexing, enhancing the functionality of the application by providing rapid, detailed search results to the users.
- **Redis**: This in-memory data structure store is used for caching and as a message broker. It improves the performance of the application by reducing the load on the main database and offering rapid data retrieval.
- **Docker**: As a platform to develop, ship, and run applications, Docker enables the application to be packaged into a standardized unit for software development, ensuring consistent behavior across various environments.
- **Background Services**: These allow the application to perform tasks in the background, improving the user experience by executing long-running tasks without interrupting the user interface. They ensure the application remains responsive and efficient, even when processing complex operations.
