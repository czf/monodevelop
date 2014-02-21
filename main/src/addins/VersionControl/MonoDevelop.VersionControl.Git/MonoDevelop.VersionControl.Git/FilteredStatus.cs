// 
// SpecificStatus.cs
//  
// Author:
//       Alan McGovern <alan@xamarin.com>
// 
// Copyright (c) 2012 Xamarin Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;

using org.eclipse.jgit.api;
using org.eclipse.jgit.lib;
using org.eclipse.jgit.treewalk;
using org.eclipse.jgit.treewalk.filter;

namespace MonoDevelop.VersionControl.Git
{
	class FilteredStatus : StatusCommand
	{
		WorkingTreeIterator iter;
		IndexDiff diff;

		IEnumerable<string> Files {
			get; set;
		}
		
		public FilteredStatus (org.eclipse.jgit.lib.Repository repository)
			: base (repository)
		{
		}
		
		public FilteredStatus (org.eclipse.jgit.lib.Repository repository, IEnumerable<string> files)
			: base (repository)
		{
			Files = files;
		}
		
		public override StatusCommand setWorkingTreeIt (WorkingTreeIterator workingTreeIt)
		{
			iter = workingTreeIt;
			return this;
		}
		
		public override Status call ()
		{
			if (iter == null)
				iter = new FileTreeIterator (repo);
			
			diff = new IndexDiff (repo, Constants.HEAD, iter);
			if (Files != null) {
				var filters = Files.Where (f => f != ".").ToArray ();
				if (filters.Length > 0)
					diff.setFilter (PathFilterGroup.createFromStrings (filters));
			}

			diff.diff ();
			return new Status (diff);
		}

		public virtual java.util.Set getIgnoredNotInIndex ()
		{
			return diff.getIgnoredNotInIndex ();
		}
	}
}

