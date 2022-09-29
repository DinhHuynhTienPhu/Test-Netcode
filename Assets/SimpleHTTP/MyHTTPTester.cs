using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleHTTP;
using System;
[System.Serializable]
public class Task
{
  public string name;
}
public class TaskID
{
  public string id;
}
public class MyHTTPTester : MonoBehaviour
{

  public string idToTestDelete;
  IEnumerator Get(string baseUrl)
  {
    Request request = new Request(baseUrl);

    Client http = new Client();
    yield return http.Send(request);
    ProcessResult(http);
  }
  [ContextMenu("TestGet")]
  public void TestGet()
  {
    StartCoroutine(Get("https://6335750f9687fb0094b5636e--dynamic-pika-2380ee.netlify.app/.netlify/functions/listtasks"));
  }

  IEnumerator Post()
  {
    // Let's say that this the object you want to create
    Task post = new Task { name = "Test" };

    // Create the request object and use the helper function `RequestBody` to create a body from JSON
    Request request = new Request("https://6335750f9687fb0094b5636e--dynamic-pika-2380ee.netlify.app/.netlify/functions/addtask")
        .Post(RequestBody.From<Task>(post));

    // Instantiate the client
    Client http = new Client();
    // Send the request
    yield return http.Send(request);

    // Use the response if the request was successful, otherwise print an error
    if (http.IsSuccessful())
    {
      Response resp = http.Response();
      Debug.Log("status: " + resp.Status().ToString() + "\nbody: " + resp.Body());
    }
    else
    {
      Debug.Log("error: " + http.Error());
    }
  }

  [ContextMenu("TestPost")]
  public void TestPost()
  {
    StartCoroutine(Post());
  }
  IEnumerator Delete(string _id)
  {
    TaskID taskid = new TaskID { id = _id };

    // Create the request object and use the helper function `RequestBody` to create a body from JSON
    Request request = new Request("https://6335750f9687fb0094b5636e--dynamic-pika-2380ee.netlify.app/.netlify/functions/deletetask")
        .Post(RequestBody.From<TaskID>(taskid));

    // Instantiate the client
    Client http = new Client();
    // Send the request
    yield return http.Send(request);

    // Use the response if the request was successful, otherwise print an error
    if (http.IsSuccessful())
    {
      Response resp = http.Response();
      Debug.Log("status: " + resp.Status().ToString() + "\nbody: " + resp.Body());
    }
    else
    {
      Debug.Log("error: " + http.Error());
    }
  }
  [ContextMenu("TestDelete")]
  public void TestDelete()
  {
    StartCoroutine(Delete(idToTestDelete));
  }

  void ProcessResult(Client http)
  {
    if (http.IsSuccessful())
    {
      Response resp = http.Response();
      Debug.Log("status: " + resp.Status().ToString() + "\nbody: " + resp.Body());
    }
    else
    {
      Debug.LogError("error: " + http.Error());
    }
  }

}
